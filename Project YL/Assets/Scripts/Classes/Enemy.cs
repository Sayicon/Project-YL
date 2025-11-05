using System.Collections;
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

    [Header("Editle")]
    [SerializeField] protected EnemyConfigSO enemyConfig;
    public bool isBoss { get; set; }
    public bool isElit { get; set; }
    public eEnemyType enemyType { get; set; }
    public float givenXp { get; set; }
    public float givenGold { get; set; }
    private Animator animator;
    private Transform playerTransform;
    private Rigidbody rb;

    /*  Damage Feedback */
    private Coroutine damageCoroutine;
    private Renderer enemyRenderer;
    public Color damageColor = Color.white;
    public float damageEffectDuration = 0.2f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (model != null)
            enemyRenderer = model.GetComponent<Renderer>();
        else
            enemyRenderer = GetComponent<Renderer>();

        InitEnemyConfig(enemyConfig);
        health = maxHealth;
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
	public virtual void InitEnemyConfig(EnemyConfigSO enemyConfig)
    {
        if (enemyConfig != null)
        {
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
            model = enemyConfig.Model;
        }
        else
        {
            Debug.LogWarning($"{name}: EnemyConfigSO atanmadi, varsayilan değerlerle başlatiliyor.");
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
    }

	public override void TakeDamage(float amount)
    {
        health -= amount;
        health = math.clamp(health, 0, maxHealth);
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(DamageEffect());
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
        if (enemyRenderer != null && enemyRenderer.material != null)
        {
            yield return DamageFlash(enemyRenderer.material, damageColor, damageEffectDuration);
        }
    }

}
