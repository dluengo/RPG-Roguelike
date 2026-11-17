using UnityEngine;

public class KeepDistance : Movement
{
    // ---------- Members ----------
    [Header("--- KeepDistance Settings ---")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float minDistance = 5f;
    [SerializeField] protected float maxDistance = 10f;

    public override Vector2 direction
    {
        get {
            Vector2 dir = (target.position - transform.position).normalized;
            float dist = Vector2.Distance(target.position, transform.position);
            if (dist < minDistance) {
                return -dir; // Move away from the target
            }
            else if (dist > maxDistance) {
                return dir; // Move towards the target
            }
            else {
                actor.faceDir = dir; // Face the target
                return Vector2.zero; // Stay in place
            }
        }
    }


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        Debug.Assert(target != null, $"object {name} doesn't have a target assigned");
    }
}
