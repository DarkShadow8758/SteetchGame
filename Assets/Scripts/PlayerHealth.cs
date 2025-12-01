using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    public int CurrentHealth { get; private set; }
    [Header("Pontos")]
    [Tooltip("Pontos concedidos a cada colisão com objetos de tag 'Damage'.")]
    public int pointsPerCollision = 10;

    private void Start()
    {
        CurrentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount = 1)
    {
        if (amount <= 0) return;
        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;
        UpdateHealthUI();
        /*if (CurrentHealth <= 0)
        {
            
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ShowFinalScore();
            }
            else
            {
                Debug.LogWarning("ScoreManager não encontrado: não foi possível mostrar a pontuação final.");
            }
        }*/
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"Vida: {CurrentHealth} / {maxHealth}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TakeDamage(1);
            if (CurrentHealth <= 0)
            {
                if (ScoreManager.Instance != null)
                {
                    Vector3 worldPos = collision.contacts.Length > 0 ? (Vector3)collision.contacts[0].point : transform.position;
                    
                    ScoreManager.Instance.AddPoints(pointsPerCollision, worldPos);
                }  
            }
                      
        }
    }
}
