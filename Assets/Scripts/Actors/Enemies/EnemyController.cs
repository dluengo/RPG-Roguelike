using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyController : ActorController
{
    // ---------- Members ---------- 
    [Header("--- EnemyController Settings ---")]
    [SerializeField] protected Transform target;
    //[SerializeField] protected float m_rangeStartAttacking;
    //public float rangeStartAttacking
    //{
    //    get => m_rangeStartAttacking;
    //    set => m_rangeStartAttacking = value;
    //}

    protected float distanceToTarget
    {
        get {
            Collider2D targetCollider = target.GetComponent<Collider2D>();
            return Vector2.Distance(targetCollider.ClosestPoint(transform.position), transform.position);
        }
    }


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();

        // NOTE: In a better design this should be if movement is MovementWithTarget
        // or similar.
        if (movement is KeepDistance) {
            KeepDistance movement = this.movement as KeepDistance;
            movement.target = target;
        }
    }

    // NOTE: Children of this class SHOULD implement the logic of the specific
    // enemy in this method. 
    protected abstract void Update();


    // ---------- Gizmos ---------- 
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, rangeStartAttacking);
        Gizmos.DrawWireSphere(transform.position, attack.range);
    }
}
