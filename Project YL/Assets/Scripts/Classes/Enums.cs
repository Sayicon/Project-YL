using UnityEngine;


[System.Serializable]
public struct Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private float multiplier;
    [SerializeField] private float total;

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
        set { total = value;  Recalculate(); }
	}

    public void Recalculate()
    {
        total = Mathf.RoundToInt(baseValue * multiplier);
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
    Zombie,
    Gianthead,
    Goblin,
    Boss
}
