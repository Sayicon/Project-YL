using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature
{
    [Header("Character Requeriments")]
    [SerializeField] private CharacterConfigSO characterConfig;
    [SerializeField] private GameObject player;
    [SerializeField] private List<string> groundTags; // Editörden tag'leri seçmek için
    private Rigidbody rb;
    [SerializeField] private Animator animator; //Animasyonlar eklenince serileştirme kaldırılcak

    [Header("Player Movement Settings")] //ilerde serileştirilemez olcak
    [Range(0, 20f)][SerializeField] private float moveSpeed = 5f;
    [Range(0, 20f)][SerializeField] private float rotationSpeed = 10f;
    [SerializeField, Range(1f, 200f)] private float acceleration = 50f;
    [Range(0, 100f)][SerializeField] private float jumpForce = 5f;
    private Vector2 moveInput;
    private bool isGrounded = true;

    // Base Stats
    private Stat projectileScale;
    private Stat collectionRange;
    private Stat chance;
    private int hasGold;

    // Config Stats
    private eCharacter characterType;
    private Stat helathRegen;
    private int projectileCount;
    private Stat attackRange;

    /* Config eklenicekler
        [] - Wapon[] weapons; //başlangıçta 1 tane 
        [] - Tome[] tomes; //başlangıçta yok
        [] - SpecialItem[] specialItems; //başlangıçta yok
        [x] - Animator animator;
        [x] - private List<string> groundTags;
        [x] - private GameObject playerModel;
    */


    //----

    void Awake()
    {
        InitPlayer(characterConfig);
    }
    void FixedUpdate()
    {
        if (rb == null) return;
        PlayerMovment();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Zıplama fonksiyonu
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void InitPlayer(CharacterConfigSO characterConfig)
    {
        if (player != null)
        {
            rb = player.GetComponent<Rigidbody>();
            if (rb == null)
                Debug.LogError("Rigidbody component not found on the player GameObject.");
        }
        else
            Debug.Log("Player GameObject is not assigned in the inspector.");
        if (characterConfig != null)
        {
            creatureName = characterConfig.CreatureName;
            creatureType = characterConfig.CreatureType;
            model = characterConfig.Model;
            groundTags = characterConfig.GroundTags;
            animator = characterConfig.Animator;
            hasGold = characterConfig.HasGold;
            characterType = characterConfig.CharacterType;
            projectileCount = characterConfig.ProjectileCount;
            maxHealth = characterConfig.MaxHealth;
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
            model = null;
            groundTags = new List<string> { "Ground", "Platform" };
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
        //devamı da  TODO
    }

    void PlayerMovment()
    {
        // Kameraya göre hareket yönünü hesapla
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 inputDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
        Vector3 desiredVelocity = inputDir * moveSpeed;

        // Haraket
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
    
    void OnCollisionEnter(Collision collision)
    {
        if (groundTags.Contains(collision.gameObject.tag))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (groundTags.Contains(collision.gameObject.tag))
        {
            isGrounded = false;
        }
    }

    //Geter Sters
    public eCharacter CharacterType { get => characterType; set => characterType = value; }
    public int HasGold { get => hasGold; set => hasGold = value; }
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }
    public Stat AttackRange { get => attackRange; set => attackRange = value; }
    public Stat ProjectileScale { get => projectileScale; set => projectileScale = value; }
    public Stat CollectionRange { get => collectionRange; set => collectionRange = value; }
    public Stat HelathRegen { get => helathRegen; set => helathRegen = value; }
    public Stat Chance { get => chance; set => chance = value; }

	
}
