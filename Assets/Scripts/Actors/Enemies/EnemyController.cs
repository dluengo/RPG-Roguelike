using UnityEngine;

public class EnemyController : ActorController
{
    // ---------- Members ----------
    [SerializeField] protected Movement move;


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        
        // NOTE: For testing purposes we set the movement script through the inspector.
        //move = GetComponent<Movement>();
        Debug.Assert(move != null, $"object {name} doesn't have a Movement component");
    }

    protected virtual void Update()
    {
        moveInput = move.direction;
    }
}
