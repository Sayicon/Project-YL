using UnityEngine;
using UnityEngine.AI;
using _Controllers;

[RequireComponent(typeof(NavMeshAgent), typeof(NPC_AnimationsControl), typeof(EnemyHealth))]
public class NPC_Controller : MonoBehaviour
{
    private NavMeshAgent agent;
    private NPC_AnimationsControl animControl; 
    private EnemyHealth health;
    private Transform playerTarget;
    
    private enum AIState { Idle, Patrolling, Chasing, Attacking, Dying }
    private AIState currentState;

    [Header("AI Ayarları")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 7f;
    [Space]
    public float detectionRange = 20f; // Oyuncuyu fark etme mesafesi
    public float attackRange = 2f;    // Saldırı mesafesi
    public float loseSightRange = 25f; // Bu mesafeye çıkarsa takibi bırakır
    [Space]
    public float attackCooldown = 3f;  // Saldırı hızı
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

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = AIState.Attacking;
        }
        else if (distanceToPlayer >= loseSightRange)
        {
            currentState = AIState.Idle;
        }
        else 
        {
            if (distanceToPlayer <= detectionRange || currentState == AIState.Chasing)
            {
                currentState = AIState.Chasing;
            }
            else
            {
                currentState = AIState.Idle;
            }
        }

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
                animControl.RunAnim(); 
                break;

            case AIState.Attacking:
                agent.isStopped = true;
                
                // Oyuncuya dönmesi için
                FaceTarget(playerTarget);
                
                // Saldırı zamanı geldiyse
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    animControl.AttackAnim();
                }
                
                break;
        }
    }
    public void PerformMeleeDamage()
    {
        // Hasar vermeden önce oyuncunun hala menzilde olup olmadığını kontrol ediyorum
        if (Vector3.Distance(transform.position, playerTarget.position) <= attackRange + 0.5f) 
        {
            Debug.Log("Mutant saldırıyor");

            PlayerHealth playerHealth = playerTarget.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(40); // 40 hasar
            }
        }
    }

    // NPC'nin hedefe dönmesi
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