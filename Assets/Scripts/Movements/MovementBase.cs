using UnityEngine;

[RequireComponent(typeof(ActorController))]
public abstract class MovementBase : MonoBehaviour
{
    // --------- Members ----------
    [Header("--- MovementBase Settings ---")]
    [SerializeField] protected float m_speed = 5f;
    public float speed
    {
        get => m_speed;
        private set => m_speed = value;
    }

    [SerializeField] protected float m_moveThreshold = 0.1f;
    public float moveThreshold { get; protected set; }

    protected ActorController actor { get; private set; }

    // NOTE: ActorController is in charge or actually moving the actor.
    // EnemyController is in charge of setting the direction for the
    // ActorController. EnemyController reads the direction from this
    // property. Child classes of Movement must implement this property
    // the way they want the actor to move.
    public Vector2 direction { get; protected set; }


    // ---------- Unity Methods ----------
    protected virtual void Awake()
    {
        actor = GetComponent<ActorController>();
        Debug.Assert(actor != null, $"object {name} doesn't have an ActorController component");
    }

    protected void OnValidate()
    {
        speed = m_speed;
    }
}
