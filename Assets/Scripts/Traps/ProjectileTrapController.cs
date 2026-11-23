using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileTrapController : TrapController
{
    // ---------- Members ----------
    [Header("ProjectileTrapController Setup")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected Vector2 projectileDirection = Vector2.right;

    [SerializeField] protected Transform m_launchPosition;


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        
        Debug.Assert(m_launchPosition != null, $"object {name} doesn't a launch position set");
    }


    // ---------- TrapController Methods ----------
    protected override void DoTrapLogic(Collider2D collision)
    {
        // NOTE: Projectile traps do nothing when the activate animation plays.
        // Instead, the animation SHOULD have an animation event that calls
        // the Shot() method.
    }


    // ---------- Animation Event Methods----------
    public void Shoot()
    {
        ProjectileController newProjectile = ProjectileController.CreateProjectile(
            projectilePrefab,
            m_launchPosition.position,
            projectileDirection,
            0,
            m_playerLayer,
            damage,
            projectileSpeed,
            knockbackForce,
            knockbackDuration);
    }
}
