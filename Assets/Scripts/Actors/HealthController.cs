using System;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class HealthController : MonoBehaviour
{
    // ---------- Members ----------
    [SerializeField] private int m_currentHealth;
    public int currentHealth
    {
        get => m_currentHealth;
        private set {
            int newHealth = Mathf.Clamp(value, 0, maxHealth);
            if (newHealth != m_currentHealth) {
                int damage = m_currentHealth - newHealth;

                m_currentHealth = newHealth;
                OnDamageTaken?.Invoke(damage);
            }

            if (m_currentHealth <= 0) {
                Destroy(gameObject);
            }
        }
    }

    [SerializeField] private int m_maxHealth = 3;
    public int maxHealth {
        get => m_maxHealth;
        private set {
            m_maxHealth = value;
            currentHealth = Mathf.Min(currentHealth, m_maxHealth);
        }
    }


    // ---------- Events ----------
    public event Action<int> OnDamageTaken;



    // ---------- Unity Methods ----------
    private void Awake()
    {
        // NOTE: Do not trigger events.
        m_currentHealth = m_maxHealth;
    }


    // ---------- Public Methods ----------
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
