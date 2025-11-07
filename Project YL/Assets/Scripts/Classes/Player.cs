using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature
{
    public static event System.Action OnPlayerDied;

    [Header("Character Requeriments")]
    [SerializeField] List<Weapon> weapons;
    [SerializeField] private CharacterConfigSO characterConfig;
    [SerializeField] private GameObject playerModelObject;
    private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Header("Player Movement Settings")]
    [Range(0, 20f)][SerializeField] private float rotationSpeed = 10f;
    [SerializeField, Range(1f, 200f)] private float acceleration = 50f;
    [Range(0, 100f)][SerializeField] private float jumpForce = 5f;
    private Vector2 moveInput;
    private bool isGrounded = true;

    [Header("Ground Check Settings")]
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -1, 0);
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Settings")]
    [SerializeField] private float jumpCooldown = 0.5f;
    private float lastJumpTime;

    private Stat projectileScale;
    private Stat collectionRange;
    private Stat chance;
    private int hasGold;
    private Sprite characterImage;
    private Dictionary<int, float> enemyLastAttackTimes = new Dictionary<int, float>();

    private eCharacter characterType;
    private Stat helathRegen;
    private int projectileCount;
    private Stat attackRange;

    [SerializeField] Weapon[] denemeWeapons;
    int deneme;

    // UI bağlantısı
    private UiManager uiManager;

    void Awake()
    {
        InitPlayer(characterConfig);

        // Sahnedeki UiManager'ı otomatik bul
        uiManager = FindFirstObjectByType<UiManager>();
        if (uiManager == null)
            Debug.LogWarning("UiManager sahnede bulunamadı! HP güncellenmeyecek.");
        else
            uiManager.SetHealth(health, maxHealth);
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        PlayerMovment();
        CheckGroundStatus();
    }

    void Update()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G) && denemeWeapons[deneme] != null)
        {
            AddWeaponToInventory(denemeWeapons[deneme]);
            deneme++;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (weapons.Count > 0)
                weapons[0].SetFiring(true, this);
        }
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.G) && denemeWeapons[deneme] != null)
        {
            AddWeaponToInventory(denemeWeapons[deneme]);
            deneme++;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            // Silah tetik testi
            if (weapons.Count > 0)
            {
                Debug.Log("AttackRange: " + AttackRange.TotalValue);
                weapons[0].SetFiring(true, this);
                Debug.Log("test");
			}
        }
	}

	public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && coyoteTimeCounter > 0f && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            coyoteTimeCounter = 0f;
            lastJumpTime = Time.time;
        }
    }

    public void AddWeaponToInventory(Weapon weapon)
    {
        if (weapon == null) return;
        weapons.Add(weapon);
        weapon.SetFiring(true, this);
        Debug.Log($"Yeni silah eklendi: {weapon.WeaponName}");
    }

    void InitPlayer(CharacterConfigSO characterConfig)
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Rigidbody component not found on the player GameObject.");

        if (characterConfig != null)
        {
            creatureName = characterConfig.CreatureName;
            creatureType = characterConfig.CreatureType;

            if (characterConfig.Model != null)
            {
                model = Instantiate(original: characterConfig.Model,
                    position: playerModelObject.transform.position - Vector3.up,
                    rotation: Quaternion.identity,
                    parent: playerModelObject.transform);
            }

            animator = characterConfig.Animator;
            hasGold = characterConfig.HasGold;
            characterType = characterConfig.CharacterType;
            characterImage = characterConfig.CharacterImage;
            projectileCount = characterConfig.ProjectileCount;
            maxHealth = characterConfig.MaxHealth;
            health = maxHealth;
            attackDamage = characterConfig.AttackDamage;
            movementSpeed = characterConfig.MovementSpeed;
            attackSpeed = characterConfig.AttackSpeed;
            projectileScale = characterConfig.ProjectileScale;
            collectionRange = characterConfig.CollectionRange;
            chance = characterConfig.Chance;
            attackRange = characterConfig.AttackRange;
            helathRegen = characterConfig.HelathRegen;
            RecalculateStats();
        }
        else
        {
            Debug.LogWarning($"{name}: CharacterConfigSO atanmamış, varsayılan değerlerle başlatılıyor.");

            creatureName = "Default Adventurer";
            creatureType = eCreatureType.Player;
            animator = GetComponent<Animator>();
            hasGold = 0;
            characterType = eCharacter.Warrior;
            projectileCount = 1;
            maxHealth = 100f;
            health = maxHealth;
            attackDamage = new Stat(10f, 1f);
            movementSpeed = new Stat(5f, 1f);
            attackSpeed = new Stat(1f, 1f);
            projectileScale = new Stat(1f, 1f);
            collectionRange = new Stat(2f, 1f);
            chance = new Stat(0.05f, 1f);
            attackRange = new Stat(1.5f, 1f);
            helathRegen = new Stat(0.5f, 1f);
        }
    }

    public override void RecalculateStats()
    {
        attackDamage.Recalculate();
        movementSpeed.Recalculate();
        attackSpeed.Recalculate();
        projectileScale.Recalculate();
        collectionRange.Recalculate();
        chance.Recalculate();
        attackRange.Recalculate();
        helathRegen.Recalculate();
    }

    void PlayerMovment()
    {
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 inputDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
        Vector3 desiredVelocity = inputDir * movementSpeed.TotalValue;

        Vector3 currentVel = rb.linearVelocity;
        Vector3 velChange = desiredVelocity - new Vector3(currentVel.x, 0f, currentVel.z);
        Vector3 force = velChange * acceleration;
        rb.AddForce(force, ForceMode.Acceleration);
        RotateModel(inputDir);
    }

    public void RotateModel(Vector3? dir = null, bool cameraRelativeIfNull = true)
    {
        Vector3 inputDir;

        if (dir.HasValue)
        {
            inputDir = dir.Value;
        }
        else
        {
            Vector3 raw = new Vector3(moveInput.x, 0f, moveInput.y);
            if (raw.sqrMagnitude < 0.0001f) return;

            if (cameraRelativeIfNull && Camera.main != null)
            {
                Vector3 camF = Camera.main.transform.forward;
                camF.y = 0f;
                camF.Normalize();

                Vector3 camR = Camera.main.transform.right;
                camR.y = 0f;
                camR.Normalize();

                inputDir = camF * raw.z + camR * raw.x;
            }
            else
            {
                inputDir = raw;
            }
        }

        inputDir.y = 0f;
        if (inputDir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(inputDir.normalized, Vector3.up);
        model.transform.rotation = Quaternion.Slerp(
            model.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(transform.position + groundCheckOffset, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log($"Player health: {health}/{maxHealth}");

        // Can UI'sini güncelle
        if (uiManager != null)
            uiManager.SetHealth(health, maxHealth);
    }

    public override void Die()
    {
        OnPlayerDied?.Invoke();

        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController != null)
            cameraController.OnPlayerDeath();

        base.Die();
        Debug.Log("GAME OVER");
    }

    public eCharacter CharacterType { get => characterType; set => characterType = value; }
    public int HasGold { get => hasGold; set => hasGold = value; }
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }
    public Stat AttackRange { get => attackRange; set => attackRange = value; }
    public Stat ProjectileScale { get => projectileScale; set => projectileScale = value; }
    public Stat CollectionRange { get => collectionRange; set => collectionRange = value; }
    public Stat HelathRegen { get => helathRegen; set => helathRegen = value; }
    public Stat Chance { get => chance; set => chance = value; }
}
