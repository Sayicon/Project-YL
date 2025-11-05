using Unity.Mathematics;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [Header("Burdan Editleme")]
    [SerializeField] public string creatureName;
    [SerializeField] public eCreatureType creatureType;
    [SerializeField] public int level;
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public Stat attackDamage;
    [SerializeField] public Stat movementSpeed;
    [SerializeField] public Stat attackSpeed;
    [SerializeField] public GameObject model;

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
        Debug.Log(name + " has Died");
        gameObject.SetActive(false);
    }
}
