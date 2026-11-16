using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileTrapController : TrapController
{
    // ---------- Members ----------
    [Header("ProjectileTrapController Setup")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected Vector2 projectileDirection = Vector2.right;

    private Transform m_launchPosition;
    private string m_launchPositionName = "LaunchPoint";


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        m_launchPosition = transform.Find(m_launchPositionName);
    }


    // ---------- TrapController Methods ----------
    protected override void DoTrapLogic(Collider2D collision)
    {
        // NOTE: Projectile traps do nothing when the activate animation plays.
        // Instead, the animation SHOULD have an animation event that calls
        // the Shot() method.
    }


    // ---------- Animation Event Methods----------
    public void Shot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, m_launchPosition.position, Quaternion.identity);
        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();

            projectile.enemyTag = m_playerTag;
            projectile.damage = damage;
            projectile.knockbackForce = knockbackForce;
            projectile.knockbackDuration = knockbackDuration;

            Rigidbody2D rb = projectileGO.GetComponent<Rigidbody2D>();
            rb.linearVelocity = projectileDirection.normalized * projectileSpeed;
        }
    }

}
