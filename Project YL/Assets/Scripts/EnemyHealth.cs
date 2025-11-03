// Dosya Yolu: Assets/Scripts/EnemyHealth.cs
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent'i durdurmak için

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    // YENİ: Gerekli bileşenlere referanslar
    private _Controllers.NPC_AnimationsControl animControl;
    private NPC_Controller npcController; // "Beyin"
    private NavMeshAgent agent;
    private bool isDead = false;

    void Awake() // Start yerine Awake
    {
        currentHealth = maxHealth;
        // YENİ: Script'leri otomatik olarak bul
        animControl = GetComponent<_Controllers.NPC_AnimationsControl>();
        npcController = GetComponent<NPC_Controller>();
        agent = GetComponent<NavMeshAgent>();
    }

    // YENİ: NPC'nin ölü olup olmadığını "beyne" bildirmek için
    public bool IsDead() 
    {
        return isDead;
    }

    // Bu metodu 'Arrow.cs' script'i çağıracak
    public void TakeDamage(float amount)
    {
        if (isDead) return; // Zaten öldüyse hasar alma

        currentHealth -= amount;
        Debug.Log(gameObject.name + " " + amount + " hasar aldı. Kalan can: " + currentHealth);

        // İsteğe bağlı: Hasar alma animasyonu
        // if (animControl != null) animControl.TakeDamageAnim(); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " öldü.");
        
        // YENİ: Ölme animasyonunu tetikle
        if (animControl != null)
        {
            animControl.DieAnim();
        }

        // Beyni ve hareketi durdur
        if (agent != null) agent.enabled = false;
        if (npcController != null) npcController.enabled = false;
        
        // Collider'ı kapat ki oklar içinden geçsin
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }

        Destroy(gameObject, 5f); // Animasyonun oynaması için 5 saniye bekle
    }
}