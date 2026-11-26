using UnityEngine;
using System.Collections.Generic;
using System;

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


    // ---------- AttackBase Interfaces ----------
    public override bool StartAttack(Vector2 direction)
    {
        if (base.StartAttack(direction)) {
            actor.SetMovementEnable(false);
            return true;
        }

        return false;
    }

    public virtual void TryHit()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = actor.enemyLayers;

        // NOTE: This is the reason we need hitArea to have a Collider2D component.
        // In order to cast a non-standard shape in the world you need to use
        // Collider2D.Cast().
        hitArea.enabled = true;
        hitArea.Cast(Vector2.zero, filter, hits, 0, true);
        hitArea.enabled = false;
        foreach (RaycastHit2D hit in hits) {
            ActorController actorHit = hit.transform.GetComponent<ActorController>();
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
        actor.SetMovementEnable(true);

        base.EndAttack();
    }
}
