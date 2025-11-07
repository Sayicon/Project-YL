using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "new RangedWeapon")]
public class RangedWeapon : Weapon
{
    [SerializeField] private GameObject explosionVFXPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool isAreaOfEffect;
    [SerializeField] private float projectileDelay = 0.1f;

    private Player player;
    private bool isFiring = false;
    private Coroutine firingCoroutine;

    // ScriptableObject her yüklendiğinde state'i sıfırla
    private void OnEnable()
    {
        isFiring = false;
        firingCoroutine = null;
        player = null;
    }

    public override void SetFiring(bool firing, Player player)
    {
        if (this.player == null)
            this.player = player;
        
        if (firing == isFiring && firingCoroutine != null) 
        {
            return;
        }
        
        if (firingCoroutine != null && this.player != null)
        {
            this.player.StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
        
        isFiring = firing;

        if (isFiring && this.player != null)
        {
            firingCoroutine = this.player.StartCoroutine(FireLoop());
        }
    }

    private IEnumerator FireLoop()
    {
        while (isFiring)
        {
            int enemyLayerMask = LayerMask.GetMask("Enemy");
            float totalAttackRange = AttackRange.TotalValue + player.AttackRange.TotalValue;
            Collider[] enemies = Physics.OverlapSphere(
                player.transform.position,
                totalAttackRange,
                enemyLayerMask
            );

            if (enemies.Length == 0)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            int totalProjectiles = player.ProjectileCount;

            if (totalProjectiles <= 0)
            {
                Debug.LogWarning("ProjectileCount 0 veya negatif!");
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            for (int i = 0; i < totalProjectiles; i++)
            {
                if (isAreaOfEffect)
                {
                    Collider randomEnemy = GetRandomEnemy(enemies);
                    if (randomEnemy != null)
                    {
                        float totalAreaRadius = Size.TotalValue + player.ProjectileScale.TotalValue;
                        
                        // Play VFX
                        player.StartCoroutine(AnimateAreaOfEffect(randomEnemy.transform.position, totalAreaRadius));

                        Collider[] hitEnemies = Physics.OverlapSphere(randomEnemy.transform.position, totalAreaRadius, enemyLayerMask);
                        
                        foreach (Collider col in hitEnemies)
                        {
                            Creature target = col.GetComponent<Creature>();
                            if (target != null)
                            {
                                float totalDamage = player.AttackDamage.TotalValue + AttackDamage.TotalValue;
                                target.TakeDamage(totalDamage);
                            }
                        }
                    }
                }
                else
                {
                    Collider randomEnemy = GetRandomEnemy(enemies);
                    if (randomEnemy != null)
                    {
                        if (projectilePrefab == null)
                        {
                            Debug.LogError("Projectile Prefab NULL! Inspector'dan atama yapın!");
                            yield break;
                        }

                        Vector3 spawnPos = player.transform.position + Vector3.up;
                        GameObject proj = GameObject.Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
                        
                        Projectile p = proj.GetComponent<Projectile>();
                        if (p != null)
                        {
                            float totalDamage = player.AttackDamage.TotalValue + AttackDamage.TotalValue;
                            Vector3 direction = (randomEnemy.transform.position - player.transform.position).normalized;
                            p.Initialize(totalDamage, totalAttackRange, direction);
                        }
                        else
                        {
                            Debug.LogError($"Projectile component bulunamadı: {proj.name}");
                        }
                    }
                }

                yield return new WaitForSeconds(projectileDelay);
            }

            float totalAttackSpeed = player.AttackSpeed.TotalValue + AttackSpeed.TotalValue;
            float waitTime = 1f / Mathf.Max(0.1f, totalAttackSpeed);
            yield return new WaitForSeconds(waitTime);
        }
        
    }

    private IEnumerator AnimateAreaOfEffect(Vector3 position, float radius)
{
        // Eğer particle prefab atanmışsa onu kullan, yoksa eski fallback sphere
        if (explosionVFXPrefab != null)
        {
            GameObject explosion = GameObject.Instantiate(explosionVFXPrefab, position, Quaternion.identity);

            // Particle'ın scale'ini alan etkisine göre ayarlayalım
            float targetScale = radius * 2f;
            explosion.transform.localScale = Vector3.one * targetScale;

            // Eğer particle sistem kendi süresine göre otomatik yok oluyorsa sorun yok,
            // yoksa süresi bitince manuel olarak yok edelim:
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                yield return new WaitForSeconds(ps.main.duration);
            }

            GameObject.Destroy(explosion);
        }
        else
        {
            // fallback: eğer prefab yoksa eski turuncu sphere efektini kullan
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;
            sphere.transform.localScale = Vector3.zero;
            sphere.GetComponent<Collider>().enabled = false;

            Renderer sphereRenderer = sphere.GetComponent<Renderer>();
            Material material = sphereRenderer.material;
            Color originalColor = new Color(1f, 0.5f, 0f, 0.2f);
            material.color = originalColor;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", originalColor);

            float duration = 0.5f;
            float timer = 0f;

            while (timer < duration)
            {
                float progress = timer / duration;
                sphere.transform.localScale = Vector3.one * Mathf.Lerp(0, radius * 2, progress);

                Color newColor = originalColor;
                newColor.a = Mathf.Lerp(originalColor.a, 0, progress);
                material.color = newColor;
                material.SetColor("_EmissionColor", newColor);

                timer += Time.deltaTime;
                yield return null;
            }

            GameObject.Destroy(sphere);
        }
    AudioManager.PlayExplosion();
}

    private Collider GetRandomEnemy(Collider[] enemies)
    {
        if (enemies == null || enemies.Length == 0)
            return null;
        
        int randomIndex = Random.Range(0, enemies.Length);
        return enemies[randomIndex];
    }

    private Collider GetClosestEnemy(Collider[] enemies, Vector3 position)
    {
        Collider closest = null;
        float minDist = Mathf.Infinity;
        
        foreach (Collider col in enemies)
        {
            if (col == null) continue;
            
            float dist = Vector3.Distance(position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col;
            }
        }
        
        return closest;
    }
}