// Dosya Yolu: Assets/Scripts/NPC_Controller.cs
using UnityEngine;
using UnityEngine.AI;
using _Controllers; // NPC_AnimationsControl'a erişim için

[RequireComponent(typeof(NavMeshAgent), typeof(NPC_AnimationsControl), typeof(EnemyHealth))]
public class NPC_Controller : MonoBehaviour
{
    // === GEREKLİ BİLEŞENLER ===
    private NavMeshAgent agent;
    private NPC_AnimationsControl animControl; 
    private EnemyHealth health;
    private Transform playerTarget;

    // === NPC'NİN BEYNİ ===
    private enum AIState
    {
        Idle,       // Duruyor
        Patrolling, // Devriye atıyor
        Chasing,    // Oyuncuyu kovalıyor (Koşma)
        Attacking,  // Oyuncuya saldırıyor
        Dying       // Ölüyor
    }
    private AIState currentState;

    [Header("AI Ayarları")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 7f;
    [Space]
    public float detectionRange = 20f; // Oyuncuyu fark etme mesafesi
    public float attackRange = 15f;    // Saldırı mesafesi (Okçu)
    public float loseSightRange = 25f; // Bu mesafeye çıkarsa takibi bırakır
    [Space]
    public float attackCooldown = 3f;  // Saldırı hızı (3 saniyede bir)
    private float lastAttackTime = -3f; 

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animControl = GetComponent<NPC_AnimationsControl>();
        health = GetComponent<EnemyHealth>();
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogError("NPC_Controller: 'Player' tag'ine sahip bir oyuncu bulunamadı!");
        }
    }

    void Update()
    {
        // === 1. ÖLÜM KONTROLÜ ===
        if (health.IsDead())
        {
            if (currentState != AIState.Dying)
            {
                currentState = AIState.Dying;
                agent.isStopped = true;
                agent.enabled = false;
                animControl.DieAnim(); 
            }
            return; 
        }

        if (playerTarget == null) return; 

        // === 2. MESAFE KONTROLÜ ===
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // === 3. DURUM (STATE) KARARLARI (GÜNCELLENDİ) ===
        
        // 1. Saldırı Menzili (En Yüksek Öncelik)
        if (distanceToPlayer <= attackRange)
        {
            currentState = AIState.Attacking;
        }
        // 2. Takibi Bırakma Menzili (Eğer 25m'den uzaktaysak, dur)
        else if (distanceToPlayer >= loseSightRange)
        {
            currentState = AIState.Idle;
        }
        // 3. Kovalama Menzili (Eğer 15m ile 25m arasındaysak)
        else 
        {
            // Eğer "algılama" menzilindeysek (örn: 15-20m) VEYA 
            // "takibi bırakma" menzilinde olsak bile (örn: 20-25m) ZATEN KOVALIYORSAK (currentState == Chasing),
            // KOVALAMAYA DEVAM ET.
            if (distanceToPlayer <= detectionRange || currentState == AIState.Chasing)
            {
                currentState = AIState.Chasing;
            }
            else
            {
                // (20m-25m arasında ama kovalamıyorduk, o zaman Idle'da kal)
                currentState = AIState.Idle;
            }
        }

        // === 4. DURUM (STATE) EYLEMLERİ ===
        switch (currentState)
        {
            case AIState.Idle:
                agent.isStopped = true;
                animControl.IdleAnim();
                break;

            case AIState.Chasing:
                agent.isStopped = false;
                agent.speed = runSpeed;
                agent.SetDestination(playerTarget.position);
                animControl.RunAnim(); // Koşma animasyonu
                break;

            case AIState.Attacking:
                agent.isStopped = true;
                
                // Oyuncuya dön
                FaceTarget(playerTarget);
                
                // Saldırı zamanı geldiyse
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    animControl.AttackAnim(); // Saldırı animasyonunu BİR KEZ tetikle
                    lastAttackTime = Time.time;
                    
                    // (Buraya NPC'nin ok fırlatma kodu eklenecek)
                }
                
                // Bir önceki "else { IdleAnim() }" bloğu, "attack/idle titremesi" 
                // yaratıyordu. Onu kaldırmıştık. 
                // Animator'deki (saldırı -> idle) geçişi bu işi halledecek.
                
                break;
        }
    }
    
    // NPC'nin hedefe dönmesini sağlar.
    private bool FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction == Vector3.zero) return true; 

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        
        float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);

        if (angleDifference < 5.0f)
        {
            return true;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed / 100f);
        return false;
    }
}