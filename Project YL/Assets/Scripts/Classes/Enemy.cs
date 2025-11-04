using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Creature
{
    [Header("Stats")]
    private bool isBoss { get; set; }
    private bool isElit { get; set; }
    private eEnemyType enemyType { get; set; }
    private float givenXp { get; set; }
    private float givenGold { get; set; }
    [SerializeField] protected EnemyConfigSO enemyConfig;
    [SerializeField] protected NavMeshAgent agent;
    private Animator animator;

	protected override void Awake()
    {
        animator = GetComponent<Animator>();
        if (enemyConfig != null)
            InitEnemyConfig(enemyConfig);
	}
	protected virtual void InitEnemyConfig(EnemyConfigSO enemyConfig)
    {
        isBoss = enemyConfig.IsBoss;
        isElit = enemyConfig.IsElit;
        enemyType = enemyConfig.EnemyType;
        givenXp = enemyConfig.GivenXp;
        givenGold = enemyConfig.GivenGold;
        if (animator != null) animator.runtimeAnimatorController = enemyConfig.AnimatorController;
	}
    protected virtual void ChasePlayer(Creature targer)
    {
        //chaseing player
    }
    protected virtual void DropXp(float multiplier)
    {
        //enemy Drop xp
    }
    
    protected virtual void DropGold(float multiplier)
    {
        //enemy Drop gold
    }

    protected virtual void DropChest(float chance)
    {
        //Enemy drop chest // şans ile orantılı eşya nadirliği
    }
    
    protected virtual void Spawn(EnemyConfigSO enemyConfig, Vector3 spawnPos)
	{
        if (enemyConfig != null)
            InitEnemyConfig(enemyConfig);
        //spawn spawnPos // polingde obje varsa poolingden yoksa yeni create edicez
	}

}
