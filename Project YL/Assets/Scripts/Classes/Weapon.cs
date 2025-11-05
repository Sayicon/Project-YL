using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite resim;
    [SerializeField] private Stat attackDamage;
    [SerializeField] private Stat attackRange;
    [SerializeField] private Stat size;
    [SerializeField] private Stat attackSpeed;

    public virtual void RecalculateStats()
    {
        attackDamage.Recalculate();
        attackRange.Recalculate();
        size.Recalculate();
        attackSpeed.Recalculate();
    }

    public abstract void SetFiring(bool firing, Player player);
    public string WeaponName { get => weaponName; set => weaponName = value; }
    public Stat AttackRange { get => attackRange; set => attackRange = value; }
    public Stat AttackDamage { get => attackDamage; set => attackDamage = value; }
    public Stat Size { get => size; set => size = value; }
    public Stat AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public Sprite Resim { get => resim; set => resim = value; }
}
