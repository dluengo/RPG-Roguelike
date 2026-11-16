using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileController : MonoBehaviour
{
    // ---------- Members ----------
    public string enemyTag;
    public int damage;
    public float knockbackForce;
    public float knockbackDuration;

    private  float lifeSpawn = 2f;


    // ---------- Unity Methods ----------
    private void Awake()
    {
        Destroy(gameObject, lifeSpawn);
    }


    // ---------- Collision Methods ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag)) {

            ActorController enemyActor = collision.GetComponent<ActorController>();
            if (enemyActor != null) {
                enemyActor.TakeDamage(damage);
                enemyActor.Knockback(transform, knockbackForce, knockbackDuration);
            }

            Destroy(gameObject);
        }
    }
}
