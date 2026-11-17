using UnityEngine;

public class Patrol : Movement
{
    // ---------- Members ----------
    [Header("--- Patrol Settings ---")]
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] protected float distanceThreshold = 0.1f;

    public override Vector2 direction
    {
        get {
            return (currPatrolPoint.position - transform.position).normalized;
        }
    }

    private int currPatrolPointIdx = 0;
    private Transform currPatrolPoint => patrolPoints[currPatrolPointIdx];


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        Debug.Assert(patrolPoints.Length > 0, $"object {name} doesn't have any patrol points assigned");
    }

    protected virtual void Update()
    {
        if (Vector2.Distance(transform.position, currPatrolPoint.position) < distanceThreshold) {
            currPatrolPointIdx = (currPatrolPointIdx + 1) % patrolPoints.Length;
        }
    }
}
