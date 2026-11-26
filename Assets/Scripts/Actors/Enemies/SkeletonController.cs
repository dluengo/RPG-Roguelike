using UnityEngine;
using UnityEngine.UIElements;

public class SkeletonController : EnemyController
{
    // ---------- Unity Methods ---------- 
    protected override void Update()
    {
        //if (distanceToTarget <= rangeStartAttacking && !isAttacking && !attack.onCooldown) {
        if (distanceToTarget <= attack.range && !isAttacking && !attack.onCooldown) {
            //Vector2 dirToTarget = (target.position - transform.position).normalized;
            //attack.StartAttack(dirToTarget);

            attack.StartAttack(target);
        }
    }
}
