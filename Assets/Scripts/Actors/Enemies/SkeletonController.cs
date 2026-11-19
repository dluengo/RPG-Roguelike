using UnityEngine;

public class SkeletonController : EnemyController
{
    // ---------- Unity Methods ---------- 
    protected override void Update()
    {
        if (distanceToTarget <= rangeStartAttacking) {
            attack.StartAttack();
        }
        else {
            // Move towards target.
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        rangeStartAttacking = m_rangeStartAttacking;
    }
}
