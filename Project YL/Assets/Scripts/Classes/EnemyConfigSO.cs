using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Config", fileName ="New Enemy Config")]
public class EnemyConfigSO : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private GameObject model;
    [SerializeField] private bool isBoss;
    [SerializeField] private bool isElit;
    [SerializeField] private eEnemyType enemyType;
    [SerializeField] private float givenXp;
    [SerializeField] private float givenGold;
    [SerializeField] private RuntimeAnimatorController animatorController;

    // TODO: Create classındaki property leri de confige ekliyicem

    public string Name => enemyName;
    public GameObject Model => model;
    public bool IsBoss => isBoss;
    public bool IsElit => isElit;
    public eEnemyType EnemyType => enemyType;
    public float GivenXp => givenXp;
    public float GivenGold => givenGold;
    public RuntimeAnimatorController AnimatorController => animatorController;
}
