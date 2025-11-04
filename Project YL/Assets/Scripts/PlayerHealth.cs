using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Oyuncu " + amount + " hasar aldı. Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Oyuncu Öldü!");
        
        if (GetComponent<PlayerController>() != null)
        {
            GetComponent<PlayerController>().enabled = false;
        }
        
        if (GetComponent<_Controllers.AnimationsControl>() != null)
        {
             // Player için die animasyonu eklenecek
             // GetComponent<_Controllers.AnimationsControl>().DieAnim();
        }

        Invoke(nameof(RestartGame), 3f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}