using Unity.Mathematics;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] protected eCreatureType type;
    protected int level;
    protected float health { get; set; }
    protected float maxHealth { get; set; }

    protected Stat attackDamage { get; set; }
    protected Stat movementSpeed { get; set; }

    protected Stat attackSpeed { get; set; }
    protected GameObject Model { get; set; }
    protected virtual void Awake()
    {
        RecalculateStats();
        health = maxHealth;
    }

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

    public void Die()
    {
        Destroy(gameObject);
    }
}
