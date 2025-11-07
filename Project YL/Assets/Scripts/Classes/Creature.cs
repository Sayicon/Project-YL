using Unity.Mathematics;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public static event System.Action<GameObject> OnCreatureDied;

    // Base Stats
    protected int level;

    // Config Stats
    protected string creatureName;
    protected eCreatureType creatureType;
    protected float health;
    protected float maxHealth;
    protected Stat attackDamage;
    protected Stat movementSpeed;
    protected Stat attackSpeed;
    protected GameObject model;

    public virtual void GiveDamage(Creature target)
    {
        if (target == null) return;
        target.TakeDamage(attackDamage.TotalValue);
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        health = math.clamp(health, 0, maxHealth);
        if (health <= 0)
            Die();
    }

    public virtual void RecalculateStats()
    {
        attackDamage.Recalculate();
        movementSpeed.Recalculate();
        attackSpeed.Recalculate();
    }

    public virtual void Die()
    {
        OnCreatureDied?.Invoke(gameObject);
        Debug.Log(name + " has Died");
        gameObject.SetActive(false);
    }
    
    //Geter Seters
    public string CreatureName { get => creatureName; set => creatureName = value; }
    public eCreatureType CreatureType { get => creatureType; set => creatureType = value; }
    public int Level { get => level; set => level = value; }
    public float Health { get => health; set => health = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public Stat AttackDamage { get => attackDamage; set => attackDamage = value; }
    public Stat MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public Stat AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public GameObject Model { get => model; set => model = value; }

}
