using UnityEngine;

public class KeepDistance : MovementBase
{
    // ---------- Members ----------
    [Header("--- KeepDistance Settings ---")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float minDistance = 5f;
    [SerializeField] protected float maxDistance = 10f;


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        Debug.Assert(target != null, $"object {name} doesn't have a target assigned");
    }

    protected virtual void Update()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        float dist = Vector2.Distance(target.position, transform.position);
        if (dist < minDistance) {
            direction = -dir; // Move away from the target
        }
        else if (dist > maxDistance) {
            direction = dir; // Move towards the target
        }
        else {
            //actor.faceDir = dir; // Face the target
            direction = Vector2.zero; // Stay in place
        }
    }
}
