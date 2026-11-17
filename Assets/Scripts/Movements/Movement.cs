using UnityEngine;

[RequireComponent(typeof(ActorController))]
public abstract class Movement : MonoBehaviour
{
    // --------- Members ----------
    // NOTE: ActorController is in charge or actually moving the actor.
    // EnemyController is in charge of setting the direction for the
    // ActorController. EnemyController reads the direction from this
    // property. Child classes of Movement must implement this property
    // the way they want the actor to move.
    public abstract Vector2 direction { get; }

    public ActorController actor { get; private set; }


    // ---------- Unity Methods ----------
    protected virtual void Awake()
    {
        actor = GetComponent<ActorController>();
        Debug.Assert(actor != null, $"object {name} doesn't have an ActorController component");
    }
}
