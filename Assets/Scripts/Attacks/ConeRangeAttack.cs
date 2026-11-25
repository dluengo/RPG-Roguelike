using UnityEngine;

public class ConeRangeAttack : RangeAttack
{
    // ---------- Members ---------- 
    [Header("--- ConeRangeAttack Settings ---")]
    [SerializeField] protected int m_numProjectiles = 3;
    public int numProjectiles
    {
        get => m_numProjectiles;
        protected set => m_numProjectiles = value;
    }

    [SerializeField] protected float m_coneAngle = 60;
    public float coneAngle
    {
        get => m_coneAngle;
        protected set => m_coneAngle = value;
    }


    // ---------- Unity Methods ---------- 
    protected override void OnValidate()
    {
        base.OnValidate();

        numProjectiles = m_numProjectiles;
        coneAngle = m_coneAngle;
    }

    // ---------- Animation Event Methods ---------- 
    public override void ShootProjectile()
    {
        Vector2 dirToTarget = (target.position - spawnPoint.position).normalized;
        float angleStep = coneAngle / numProjectiles;
        float baseAngle = -coneAngle/2 + angleStep/2;

        for (int i = 0; i < numProjectiles; i++) {

            float projectileAngle = baseAngle + angleStep * i;
            Vector2 projectileDir = Quaternion.Euler(0, 0, projectileAngle) * dirToTarget;

            ShootProjectile(projectileDir);
        }
    }
}
