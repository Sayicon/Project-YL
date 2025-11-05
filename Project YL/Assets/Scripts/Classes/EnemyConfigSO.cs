using UnityEngine;

[CreateAssetMenu(menuName ="New Enemy Config", fileName ="Enemy Config")]
public class EnemyConfigSO : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private bool isBoss;
    [SerializeField] private bool isElit;
    [SerializeField] private eCreatureType creatureType;
    [SerializeField] private eEnemyType enemyType;
    [SerializeField] private LayerMask climbableLayer;
    [SerializeField] private int level;
    [SerializeField] private float maxHealth;
    [SerializeField] private float givenXp;
    [SerializeField] private float givenGold;
    [SerializeField] private Stat attackDamage;
    [SerializeField] private Stat movementSpeed;
    [SerializeField] private Stat attackSpeed;
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private GameObject model;

    public LayerMask ClimbableLayer => climbableLayer;
    public Stat AttackSpeed => attackSpeed;
    public Stat MovementSpeed => movementSpeed;
    public Stat AttackDamage => attackDamage;
    public float MaxHealth => maxHealth;
    public int Level => level;
    public eCreatureType CreatureType => creatureType;
    public string EnemyName => enemyName;
    public GameObject Model => model;
    public bool IsBoss => isBoss;
    public bool IsElit => isElit;
    public eEnemyType EnemyType => enemyType;
    public float GivenXp => givenXp;
    public float GivenGold => givenGold;
    public RuntimeAnimatorController AnimatorController => animatorController;
}
