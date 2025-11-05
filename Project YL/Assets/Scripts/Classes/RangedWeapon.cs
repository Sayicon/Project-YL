using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "new RangedWeapon")]
public class RangedWeapon : Weapon
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool isAOE;
    [SerializeField] private float aoeRadius = 2f;
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
        Debug.Log($"SetFiring called: firing={firing}, isFiring={isFiring}");
        Debug.Log($"AttackRange: {AttackRange.TotalValue}");
        Debug.Log($"Projectile Prefab: {projectilePrefab}");
        
        if (this.player == null)
            this.player = player;
        
        // Eğer zaten aynı durumdaysa VE coroutine çalışıyorsa, return et
        if (firing == isFiring && firingCoroutine != null) 
        {
            Debug.Log("Same state and coroutine already running, returning");
            return;
        }
        
        // Önceki coroutine'i durdur
        if (firingCoroutine != null && this.player != null)
        {
            this.player.StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
        
        isFiring = firing;

        if (isFiring && this.player != null)
        {
            Debug.Log("Starting firing coroutine");
            firingCoroutine = this.player.StartCoroutine(FireLoop());
        }
        else
        {
            Debug.Log("Stopping firing");
        }
    }

    private IEnumerator FireLoop()
    {
        Debug.Log("FireLoop STARTED");
        
        while (isFiring)
        {
            // Layer kontrolü
            int enemyLayerMask = LayerMask.GetMask("Enemy");
            Debug.Log($"Enemy Layer Mask: {enemyLayerMask}");
            
            // Hedefleri tespit et
            Collider[] enemies = Physics.OverlapSphere(
                player.transform.position, 
                AttackRange.TotalValue, 
                enemyLayerMask
            );
            
            Debug.Log($"Enemies found: {enemies.Length} | AttackRange: {AttackRange.TotalValue} | Position: {player.transform.position}");

            if (enemies.Length == 0)
            {
                Debug.Log("Düşman bulunamadı, bekleniyor...");
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            int totalProjectiles = player.ProjectileCount;
            Debug.Log($"Total projectiles to fire: {totalProjectiles}");

            if (totalProjectiles <= 0)
            {
                Debug.LogWarning("ProjectileCount 0 veya negatif!");
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            // Her mermi rastgele bir düşmana gitsin
            for (int i = 0; i < totalProjectiles; i++)
            {
                if (isAOE)
                {
                    // Alan etkisi
                    Vector3 aoePos = player.transform.position + player.transform.forward * AttackRange.TotalValue;
                    Collider[] hitEnemies = Physics.OverlapSphere(aoePos, aoeRadius, enemyLayerMask);
                    Debug.Log($"AOE hit {hitEnemies.Length} enemies");
                    
                    foreach (Collider col in hitEnemies)
                    {
                        Creature target = col.GetComponent<Creature>();
                        if (target != null)
                        {
                            float totalDamage = player.AttackDamage.TotalValue + AttackDamage.TotalValue;
                            Debug.Log($"Dealing {totalDamage} damage to {target.name}");
                            target.TakeDamage(totalDamage);
                        }
                    }
                }
                else
                {
                    // RASTGELE bir düşman seç
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
                        Debug.Log($"Projectile spawned at {spawnPos}");
                        
                        Projectile p = proj.GetComponent<Projectile>();
                        if (p != null)
                        {
                            float totalDamage = player.AttackDamage.TotalValue + AttackDamage.TotalValue;
                            float totalRange = AttackRange.TotalValue + player.AttackRange.TotalValue;
                            Vector3 direction = (randomEnemy.transform.position - player.transform.position).normalized;
                            
                            Debug.Log($"Initializing projectile: Damage={totalDamage}, Range={totalRange}, Target={randomEnemy.name}");
                            p.Initialize(totalDamage, totalRange, direction);
                        }
                        else
                        {
                            Debug.LogError($"Projectile component bulunamadı: {proj.name}");
                        }
                    }
                    else
                    {
                        Debug.Log("Random enemy null!");
                    }
                }
                
                yield return new WaitForSeconds(projectileDelay);
            }

            // AttackSpeed ile orantılı bekleme
            float totalAttackSpeed = player.AttackSpeed.TotalValue + AttackSpeed.TotalValue;
            float waitTime = 1f / Mathf.Max(0.1f, totalAttackSpeed); // Division by zero koruması
            Debug.Log($"Waiting {waitTime} seconds before next attack cycle");
            yield return new WaitForSeconds(waitTime);
        }
        
        Debug.Log("FireLoop ENDED");
    }

    private Collider GetRandomEnemy(Collider[] enemies)
    {
        if (enemies == null || enemies.Length == 0)
            return null;
        
        // Rastgele bir düşman seç
        int randomIndex = Random.Range(0, enemies.Length);
        Collider randomEnemy = enemies[randomIndex];
        
        if (randomEnemy != null)
            Debug.Log($"Random enemy selected: {randomEnemy.name}");
        
        return randomEnemy;
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
        
        if (closest != null)
            Debug.Log($"Closest enemy: {closest.name} at distance {minDist}");
        
        return closest;
    }
}