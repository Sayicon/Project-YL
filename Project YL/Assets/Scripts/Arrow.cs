using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour
{
    public float speed = 40f;   // Okun hızı
    public float damage = 25f;  // Okun hasarı
    public float lifetime = 10f; // Ok 10 saniye sonra yok olur

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.isKinematic = false;
        rb.useGravity = true; // yerçekimi
        
        rb.linearVelocity = transform.forward * speed;
        
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == enemyLayer)
        {
            return; 
        }
        
        rb.linearVelocity = Vector3.zero;

        rb.isKinematic = true;

        EnemyHealth targetEnemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damage);
        }
        
        PlayerHealth targetPlayer = collision.gameObject.GetComponent<PlayerHealth>();
        if (targetPlayer != null)
        {
            targetPlayer.TakeDamage(damage);
        }

        transform.SetParent(collision.transform);

        Destroy(this, 1f);
        Destroy(GetComponent<Collider>(), 1f);
    }
}