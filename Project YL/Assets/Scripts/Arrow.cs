// Dosya Yolu: Assets/Scripts/Arrow.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour
{
    // Okun ayarları
    public float speed = 40f;   // Okun ne kadar hızlı gideceği
    public float damage = 25f;  // Okun ne kadar hasar vereceği
    public float lifetime = 10f; // Okun 10 saniye sonra kendini yok etmesi (eğer bir şeye çarpmazsa)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Okun fizik motorunu ayarla
        rb.isKinematic = false;
        rb.useGravity = true; // Gerçekçilik için yer çekimi kullanabilir
        
        // Oku fırlat!
        // (Bu script'in 'transform.forward' yönü, onu kim spawn ettiyse onun yönü olacaktır)
        rb.linearVelocity = transform.forward * speed;
        
        // Oku 'lifetime' saniye sonra yok et
        Destroy(gameObject, lifetime);
    }

    // Bir şeye çarptığında
    void OnCollisionEnter(Collision collision)
    {
        // --- YENİ GÜVENLİK KONTROLÜ ---
        // Çarptığımız nesnenin katmanı "Player" mı?
        // (LayerMask.NameToLayer("Player") komutu, "Player" katmanının numarasını bulur)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Eğer "Player" ise, bu biziz (atıcı).
            // Bu çarpışmayı YOKSAY ve uçmaya devam et.
            return; 
        }
        
        // --- Çarpışma Gerçekleşti (Player dışında bir şeye) ---

        // 1. Önce okun fiziksel hareketini (hızını) durdur.
        rb.linearVelocity = Vector3.zero;

        // 2. Hareketi durduktan sonra, onu fizik motorundan çıkar (kinematik yap).
        rb.isKinematic = true;

        // Çarptığımız nesnede "EnemyHealth" (Düşman Canı) script'i var mı diye kontrol et
        EnemyHealth targetHealth = collision.gameObject.GetComponent<EnemyHealth>();
        
        if (targetHealth != null)
        {
            // Varsa, ona hasar ver
            targetHealth.TakeDamage(damage);
        }

        // Oku, çarptığı nesneye "sapla" (onu parent yap)
        transform.SetParent(collision.transform);

        // Bu script'i ve collider'ı 1 saniye sonra devre dışı bırak ki tekrar hasar vermesin
        Destroy(this, 1f);
        Destroy(GetComponent<Collider>(), 1f);
    }
}