using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileController : MonoBehaviour
{
    // ---------- Members ----------
    [Header("--- ProjectileController Settings ---")]
    [SerializeField] protected LayerMask damageLayer;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float knockbackForce;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected float lifeSpawn = 2f;
    [SerializeField] protected float rotationOffset;

    private Vector2 direction;


    // ---------- Unity Methods ----------
    private void Start()
    {
        Destroy(gameObject, lifeSpawn);
    }


    // ---------- Collision Methods ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayerMask = 1 << collision.gameObject.layer;
        if ((damageLayer.value & collisionLayerMask) != 0) {

            ActorController enemyActor = collision.GetComponent<ActorController>();
            if (enemyActor != null) {
                enemyActor.TakeDamage(damage);
                enemyActor.Knockback(transform, knockbackForce, knockbackDuration);
            }

            Destroy(gameObject);
        }
    }

    // ---------- Class Methods ---------- 
    public static ProjectileController CreateProjectile(
        GameObject projectilePrefab,
        Vector2 spawnCoords,
        Vector2 direction,
        float rotationOffset,
        LayerMask damageLayer,
        int damage = 1,
        float speed = 10,
        float knockbackForce = 0,
        float knockbackDuration = 0)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += rotationOffset;

        ProjectileController newProjectile = Instantiate(
            projectilePrefab,
            spawnCoords,
            Quaternion.Euler(0, 0, angle)
            ).GetComponent<ProjectileController>();
        newProjectile.direction = direction.normalized;
        newProjectile.speed = speed;
        newProjectile.damageLayer = damageLayer;
        newProjectile.damage = damage;
        newProjectile.knockbackForce = knockbackForce;
        newProjectile.knockbackDuration = knockbackDuration;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = newProjectile.direction * speed;

        return newProjectile;
    }
}
