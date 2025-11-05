using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float range;
    private Vector3 direction;
    [SerializeField] private float speed = 10f;
    private Vector3 startPos;

    public void Initialize(float damage, float range, Vector3 dir)
    {
        this.damage = damage;
        this.range = range;
        this.direction = dir.normalized;
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        // Menzil kontrolü
        if (Vector3.Distance(startPos, transform.position) >= range)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Creature target = other.GetComponent<Creature>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
