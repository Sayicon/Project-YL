using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DummyScript : MonoBehaviour
{
    [Header("Chase Settings")]
    public bool canChase = false; // Checkbox to enable/disable chasing
    public Transform target; // Target to chase
    public float chaseSpeed = 3f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar; // Reference to the HealthBar script

    [Header("Damage Feedback")]
    public Renderer dummyRenderer; // Renderer for visual feedback
    public Color damageColor = Color.white;
    public float damageEffectDuration = 0.2f;
    private Rigidbody rb;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        //Playeri bul
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Cache Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //test damage
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10);
        }
        //test chase
        if (Input.GetKeyDown(KeyCode.C))
        {
            canChase = !canChase;
        }

        // Handle chasing logic
        if (canChase && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * chaseSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduce health
        currentHealth -= damage;
        StartCoroutine(DamageEffect());
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health bar
        healthBar.SetHealth(currentHealth);

        // Trigger damage feedback
        if (dummyRenderer != null && dummyRenderer.material != null)
        {
            StopAllCoroutines();
            StartCoroutine(DamageFlash(dummyRenderer.material, damageColor, damageEffectDuration));
        }

        // Check if health is zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlash(Material mat, Color flashColor, float duration)
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", flashColor);
        yield return new WaitForSeconds(duration);
        mat.DisableKeyword("_EMISSION");
    }

    private IEnumerator DamageEffect()
    {
        if (dummyRenderer != null && dummyRenderer.material != null)
        {
            yield return DamageFlash(dummyRenderer.material, damageColor, damageEffectDuration);
        }
    }

    private void Die()
    {
        // Handle death logic (e.g., disable the object, play animation, etc.)
        Debug.Log("Dummy has died.");
        gameObject.SetActive(false);
    }
}
