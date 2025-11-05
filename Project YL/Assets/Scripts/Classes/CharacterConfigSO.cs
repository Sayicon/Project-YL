using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfigSO", menuName = "New CharacterConfig")]
public class CharacterConfigSO : ScriptableObject
{
    [SerializeField] private string creatureName;
    [SerializeField] private eCreatureType creatureType;
	[SerializeField] private GameObject model;
	
	[SerializeField] private List<string> groundTags;
	[SerializeField] private Animator animator;
    [SerializeField] private int hasGold;
    [SerializeField] private eCharacter characterType;
	
	[SerializeField] private int projectileCount;
	[SerializeField] private float maxHealth;
	[SerializeField] private Stat attackDamage;
    [SerializeField] private Stat movementSpeed;
	[SerializeField] private Stat attackSpeed;
	[SerializeField] private Stat projectileScale;
    [SerializeField] private Stat collectionRange;
	[SerializeField] private Stat chance;
	[SerializeField] private Stat attackRange;
	[SerializeField] private Stat helathRegen;

	// Geters

	public string CreatureName => creatureName;
	public eCreatureType CreatureType => creatureType;
	public float MaxHealth => maxHealth;
	public Stat AttackDamage => attackDamage;
	public Stat MovementSpeed => movementSpeed;
	public Stat AttackSpeed => attackSpeed;
	public GameObject Model => model;

	public List<string> GroundTags => groundTags;
	public Animator Animator => animator;
	public Stat ProjectileScale => projectileScale;
	public Stat CollectionRange => collectionRange;
	public Stat Chance => chance;
	public int HasGold => hasGold;
	public eCharacter CharacterType => characterType;
	public Stat HelathRegen => helathRegen;
	public int ProjectileCount => projectileCount;
	public Stat AttackRange => attackRange;

}
