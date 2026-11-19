using UnityEngine;
using System.Collections.Generic;

public class MeleeAttack : AttackBase
{
    // ---------- Members ----------
    [SerializeField] protected Collider2D hitArea;


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(hitArea != null, $"object {name} doesn't have a HitArea Collider2D");

        if (hitArea != null) {
            hitArea.enabled = false;
        }
    }

    protected virtual void OnEnable()
    {
        actor.OnAttackAnimationEnd += AttackEndHandler;
    }

    protected virtual void OnDisable()
    {
        actor.OnAttackAnimationEnd -= AttackEndHandler;
    }


    // ---------- Animation Event Methods ----------
    public override void StartAttack()
    {
        if (!actor.isAttacking) {
            actor.SetMovementEnable(false);
            actor.Attack();
        }
    }

    public override void TryHit()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = actor.enemyLayer;

        // NOTE: This is the reason we need hitArea to have a Collider2D component.
        // In order to cast a non-standard shape in the world you need to use
        // Collider2D.Cast().
        hitArea.enabled = true;
        hitArea.Cast(Vector2.zero, filter, hits, 0, true);
        hitArea.enabled = false;
        foreach (RaycastHit2D hit in hits) {
            ActorController actorHit = hit.transform.gameObject.GetComponent<ActorController>();
            if (actorHit) {
                actorHit.TakeDamage(damage);
            }
            else {
                Debug.LogWarning($"{name}: TryHit(): object hit {actorHit.name} doesn't have ActorController");
            }
        }
    }

    public override void EndAttack()
    {
        
    }


    // ---------- Collision Detection Methods ----------
    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    ActorController actorHit = collision.GetComponent<ActorController>();
    //    if (actorHit != null) {
    //        actorHit.TakeDamage(damage);
    //    }
    //}


    // ---------- Event Handlers ---------- 
    private void AttackEndHandler()
    {
        actor.SetMovementEnable(true);
    }
}
