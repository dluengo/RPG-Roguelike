using UnityEngine;

public class EnemyController : ActorController
{
    // ---------- Members ----------
    protected Movement move;


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        
        move = GetComponent<Movement>();
        Debug.Assert(move != null, $"object {name} doesn't have a Movement component");
    }

    protected virtual void Update()
    {
        moveInput = move.direction;
    }
}
