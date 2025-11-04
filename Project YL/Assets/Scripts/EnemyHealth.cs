using UnityEngine;
using UnityEngine.AI; 

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private _Controllers.NPC_AnimationsControl animControl;
    private NPC_Controller npcController; 
    private NavMeshAgent agent;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animControl = GetComponent<_Controllers.NPC_AnimationsControl>();
        npcController = GetComponent<NPC_Controller>();
        agent = GetComponent<NavMeshAgent>();
    }

    public bool IsDead() 
    {
        return isDead;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; 

        currentHealth -= amount;
        Debug.Log(gameObject.name + " " + amount + " hasar aldı. Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " öldü.");
        
        if (animControl != null)
        {
            animControl.DieAnim();
        }

        if (agent != null) agent.enabled = false;
        if (npcController != null) npcController.enabled = false;
        
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }

        Destroy(gameObject, 5f); 
    }
}