using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Creature
{
    [Header("Chasing Settings")]
    [SerializeField] bool chase;
    [SerializeField] LayerMask climbableLayer;
    [SerializeField] float climbForce = 20f;
    [SerializeField] float wallCheckDistance = 1f;
    [SerializeField] private float rotationSpeed = 10f;

    // stats
    [SerializeField] protected EnemyConfigSO enemyConfig;
    private bool isBoss;
    private bool isElit;
    private eEnemyType enemyType;
    private float givenXp;
    private float givenGold;

    [Header("Elite Settings")]
    private bool isElite = false;
    [SerializeField] private float eliteHealthMultiplier = 2.5f;
    [SerializeField] private float eliteDamageMultiplier = 2f;
    [SerializeField] private float eliteScaleMultiplier = 1.25f;

    // Script requariments
    private Animator animator;
    private Transform playerTransform;
    private Rigidbody rb;

    /*  Damage Feedback */
    private Coroutine damageCoroutine;
    private Renderer[] enemyRenderers; // Tekil yerine array kullanıyoruz
    public Color damageColor = Color.white;
    public float damageEffectDuration = 0.2f;

    private float lastAttackTime;

    void Awake()
    {
        Player.OnPlayerDied += HandlePlayerDeath;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        health = maxHealth;
    }

    void OnDestroy()
    {
        Player.OnPlayerDied -= HandlePlayerDeath;
    }

    void Update()
    {
        //test damage
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(10);
        //test chase
        if (Input.GetKeyDown(KeyCode.C)) chase = !chase;
        // Handle chasing logic
    }

    void FixedUpdate()
    {
        if (chase) ChasePlayer(playerTransform);
    }

    [SerializeField] private GameObject enemyModelObject;


    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
    }

    public virtual void InitEnemyConfig(EnemyConfigSO enemyConfig)
    {

        if (enemyConfig != null)
        {
            chase = true;
            climbableLayer = enemyConfig.ClimbableLayer;
            creatureName = enemyConfig.EnemyName;
            isBoss = enemyConfig.IsBoss;
            isElit = enemyConfig.IsElit;
            enemyType = enemyConfig.EnemyType;
            givenXp = enemyConfig.GivenXp;
            givenGold = enemyConfig.GivenGold;
            if (animator != null) animator.runtimeAnimatorController = enemyConfig.AnimatorController;
            creatureType = enemyConfig.CreatureType;
            level = enemyConfig.Level;
            maxHealth = enemyConfig.MaxHealth;
            attackDamage = enemyConfig.AttackDamage;
            movementSpeed = enemyConfig.MovementSpeed;
            attackSpeed = enemyConfig.AttackSpeed;

            if (enemyConfig.Model != null)
            {
                model = Instantiate(original: enemyConfig.Model, position: enemyModelObject.transform.position - Vector3.up,
                        rotation: Quaternion.Euler(0, 0, 0), parent: enemyModelObject.transform);

                // Tüm alt objelerdeki Renderer'ları bul
                enemyRenderers = model.GetComponentsInChildren<Renderer>();
            }
        }
        else
        {
            Debug.LogWarning($"{name}: EnemyConfigSO atanmadi, varsayilan değerlerle başlatiliyor.");
            chase = true;
            climbableLayer = LayerMask.GetMask("Moovable");
            isBoss = false;
            isElit = false;
            enemyType = eEnemyType.None;
            givenXp = 10f;
            givenGold = 5f;
            creatureType = eCreatureType.Enemy;
            level = 1;
            health = maxHealth = 100f;
            attackDamage = new Stat(10f, 1f);
            movementSpeed = new Stat(3.5f, 1f);
            attackSpeed = new Stat(1f, 1f);
        }

        RecalculateStats();
        health = maxHealth;

        if (isElite)
        {
            maxHealth *= eliteHealthMultiplier;
            health = maxHealth;
            attackDamage.BaseValue *= eliteDamageMultiplier;
            transform.localScale *= eliteScaleMultiplier;
        }
    }

    public void SetElite(bool isElite)
    {
        this.isElite = isElite;
    }

    public override void TakeDamage(float amount)
    {
        health -= amount;
        health = math.clamp(health, 0, maxHealth);

        // Eğer bu düşman Skeleton tipindeyse, ışık efektini atla
        if (enemyType != eEnemyType.Skeleton)
        {
            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);
            damageCoroutine = StartCoroutine(DamageEffect());
        }

        if (health <= 0)
            Die();
    }


    public void ChasePlayer(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;

        // Duvar kontrolü
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, wallCheckDistance, climbableLayer))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) < 0.5f)
            {
                // Yukarı doğru tırmanma kuvveti uygula
                rb.AddForce(Vector3.up * climbForce, ForceMode.Acceleration);
            }
        }

        // İleri hareket
        rb.MovePosition(transform.position + direction * movementSpeed.TotalValue * Time.deltaTime);

        // Oyuncuya doğru dönme
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void DropXp(float multiplier)
    {
        //enemy Drop xp
    }

    public void DropGold(float multiplier)
    {
        //enemy Drop gold
    }

    public void DropChest(float chance)
    {
        //Enemy drop chest // şans ile orantılı eşya nadirliği
    }

    public void Spawn(EnemyConfigSO enemyConfig, Vector3 spawnPos)
    {
        if (enemyConfig != null)
            InitEnemyConfig(enemyConfig);
        //spawn spawnPos // polingde obje varsa poolingden yoksa yeni create edicez
    }

    public override void Die()
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        ResetEmission(); // Call this before deactivating the GameObject

        float xpMultiplier = isElite ? 2f : 1f;
        float goldMultiplier = isElite ? 2f : 1f;

        // Drop loot based on stats
        DropXp(xpMultiplier);
        DropGold(goldMultiplier);
        if (isBoss || (isElite && UnityEngine.Random.value < 0.25f))
        {
            DropChest(1f);
        }

        if (isElite)
        {
            transform.localScale /= eliteScaleMultiplier;
        }

        base.Die();
    }

    private IEnumerator DamageFlash(Material mat, Color flashColor, float duration)
    {
        mat.EnableKeyword("_EMISSION");

        Color originalColor = mat.GetColor("_EmissionColor");
        float timer = 0f;

        // Parlamaya doğru geçiş
        while (timer < duration / 2f)
        {
            timer += Time.deltaTime;
            float t = timer / (duration / 2f);
            mat.SetColor("_EmissionColor", Color.Lerp(originalColor, flashColor, t));
            yield return null;
        }

        // Geri solma
        timer = 0f;
        while (timer < duration / 2f)
        {
            timer += Time.deltaTime;
            float t = timer / (duration / 2f);
            mat.SetColor("_EmissionColor", Color.Lerp(flashColor, originalColor, t));
            yield return null;
        }

        mat.SetColor("_EmissionColor", originalColor);
        mat.DisableKeyword("_EMISSION");
    }

    private IEnumerator DamageEffect()
    {
        if (enemyRenderers != null && enemyRenderers.Length > 0)
        {
            // Tüm renderer'lar için coroutine'leri başlat
            List<Coroutine> flashCoroutines = new List<Coroutine>();

            foreach (Renderer renderer in enemyRenderers)
            {
                if (renderer != null && renderer.material != null)
                {
                    Coroutine flashCo = StartCoroutine(DamageFlash(renderer.material, damageColor, damageEffectDuration));
                    flashCoroutines.Add(flashCo);
                }
            }

            // Tüm efektlerin bitmesini bekle
            yield return new WaitForSeconds(damageEffectDuration);
        }
    }

    private void HandlePlayerDeath()
    {
        chase = false;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time > lastAttackTime + (1f / attackSpeed.TotalValue))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    GiveDamage(player);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    //Geter Seters
    public bool IsBoss { get => isBoss; set => isBoss = value; }
    public bool IsElit { get => isElit; set => isElit = value; }
    public eEnemyType EnemyType { get => enemyType; set => enemyType = value; }
    public float GivenXp { get => givenXp; set => givenXp = value; }
    public float GivenGold { get => givenGold; set => givenGold = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public Transform PlayerTransform { get => playerTransform; set => playerTransform = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    private void ResetEmission()
    {
        if (enemyRenderers == null || enemyRenderers.Length == 0) return;

        foreach (var renderer in enemyRenderers)
        {
            if (renderer != null && renderer.material != null)
            {
                if (renderer.material.HasProperty("_EMISSION") && renderer.material.IsKeywordEnabled("_EMISSION"))
                {
                    renderer.material.DisableKeyword("_EMISSION");
                    renderer.material.SetColor("_EmissionColor", Color.black); // Ensure it's fully off
                }
            }
        }
    }
}