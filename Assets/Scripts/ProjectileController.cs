using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileController : MonoBehaviour
{
    // ---------- Members ----------
    [Header("--- ProjectileController Settings ---")]
    [SerializeField] protected LayerMask damageLayers;
    [SerializeField] protected int damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float knockbackForce;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected float lifeSpawn = 2f;
    [SerializeField] protected float rotationOffset;

    private Vector2 direction;
    private GameObject throwerGO;


    // ---------- Unity Methods ----------
    private void Start()
    {
        Destroy(gameObject, lifeSpawn);
    }


    // ---------- Collision Methods ----------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject triggerGO = collision.gameObject;
        ActorController triggerActor = collision.GetComponent<ActorController>();

        
        TrapController throwerTrap = null;
        ActorController throwerActor = throwerGO.GetComponent<ActorController>();
        if (throwerActor == null) {
            throwerTrap = throwerGO.GetComponent<TrapController>();
        }


        // NOTE: This can be simplified in two if statements, but for now i rather
        // leave it as is as I think it is easier to follow the logic.
        if (throwerTrap != null) {
            if (triggerActor != null) {
                if (throwerTrap.IsEnemy(triggerActor)) {
                    triggerActor.TakeDamage(damage);
                    triggerActor.Knockback(transform, knockbackForce, knockbackDuration);
                    Destroy(gameObject);
                }
            }
        }
        else if (throwerActor != null) {
            if (triggerActor != null) {
                if (throwerActor.IsEnemy(triggerActor)) {
                    triggerActor.TakeDamage(damage);
                    triggerActor.Knockback(transform, knockbackForce, knockbackDuration);
                    Destroy(gameObject);
                }
            }
        }
        else {
            Debug.LogWarning($"obj {name}: throwerGO is not either TrapController or ActorController!");
        }
    }

    // ---------- Class Methods ---------- 
    public static ProjectileController CreateProjectile(
        GameObject projectilePrefab,
        Vector2 spawnCoords,
        Vector2 direction,
        float rotationOffset,
        GameObject thrower,
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
        newProjectile.throwerGO = thrower;
        newProjectile.damage = damage;
        newProjectile.knockbackForce = knockbackForce;
        newProjectile.knockbackDuration = knockbackDuration;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = newProjectile.direction * speed;

        return newProjectile;
    }
}
