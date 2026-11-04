using UnityEngine;

public class SpikeController : MonoBehaviour
{
    // ---------- Members ----------
    [SerializeField] int m_damage = 1;
    public int damage
    {
        get => m_damage;
        private set => m_damage = value;
    }

    [SerializeField] float m_force = 5f;
    public float force
    {
        get => m_force;
        private set => m_force = value;
    }

    [SerializeField] float m_duration = 0.5f;
    public float duration
    {
        get => m_duration;
        private set => m_duration = value;
    }


    // ---------- Collision Methods ----------
    public void OnTriggerEnter2D(Collider2D collision)
    {
        HealthController health = collision.GetComponent<HealthController>();
        if (health != null) {
            health.TakeDamage(damage);
        }

        ActorController actor = collision.GetComponent<ActorController>();
        if (actor != null) {
            actor.Knockback(transform, force, duration);
        }
    }
}
