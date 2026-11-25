using UnityEngine;

public class RangeAttack : AttackBase
{
    // ---------- Members ----------
    [Header("--- RangeAttack Settings ---")]
    [SerializeField] protected float m_projectileSpeed = 10;
    public float projectileSpeed
    {
        get => m_projectileSpeed;
        protected set => m_projectileSpeed = value;
    }

    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected float rotationOffset;


    // ---------- Unity Methods ---------- 
    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(projectilePrefab != null, $"object {name} doesn't have a projectile prefab set");
        Debug.Assert(spawnPoint != null, $"object {name} doesn't have a spawn point set for projectile creation");
    }


    // ---------- Attack Methods ---------- 
    public override bool StartAttack(Vector2 direction)
    {
        bool canAttack = base.StartAttack(direction);
        if (canAttack) {
            actor.SetMovementEnable(false);
        }

        return canAttack;
    }

    public override void EndAttack()
    {
        actor.SetMovementEnable(true);
        base.EndAttack();
    }


    // ---------- Animation Event Method ---------- 
    public virtual void ShootProjectile()
    {
        Vector2 projectileDir = target != null ?
            (target.position - spawnPoint.position).normalized :
            actor.faceDir;

        ShootProjectile(projectileDir);
    }


    // ---------- Shoot projectile method ---------- 
    public virtual void ShootProjectile(Vector2 direction)
    {
        ProjectileController newProjectile = ProjectileController.CreateProjectile(
            projectilePrefab,
            spawnPoint.position,
            direction,
            rotationOffset,
            actor.enemyLayer,
            damage,
            projectileSpeed,
            knockbackForce,
            knockbackDuration);
    }
}
