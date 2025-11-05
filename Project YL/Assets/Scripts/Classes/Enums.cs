using UnityEngine;


[System.Serializable]
public struct Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private float multiplier;
    [SerializeField] private float total;

    public Stat(float baseValue, float multiplier)
    {
        this.baseValue = baseValue;
        this.multiplier = multiplier;
        this.total = baseValue * multiplier;
    }


    public float BaseValue
    {
        get => baseValue;
        set { baseValue = value; Recalculate(); }
    }

    public float Multiplier
    {
        get => multiplier;
        set { multiplier = value; Recalculate(); }
    }

    public float TotalValue
	{
        get => total;
        set => total = value;
	}

    public void Recalculate()
    {
        total = baseValue * multiplier;
    }
}
public enum eCreatureType
{
    None,
    Human,
    Enemy,
}

public enum eEnemyType
{
    None,
    Zombie,
    Gianthead,
    Goblin,
    Boss
}
