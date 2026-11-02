using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ActorController : MonoBehaviour
{
    // --- Members ---
    [SerializeField] protected float m_moveSpeed = 5f;
    public float moveSpeed {
        get => m_moveSpeed;
        private set => m_moveSpeed = value;
    }

    protected Rigidbody2D rb;

    // To be set by the class implementing ActorController. That class defines
    // how the actor moves, then this class handles the actual movement.
    private Vector2 m_moveInput = Vector2.zero;
    public Vector2 moveInput
    {
        get => m_moveInput;
        protected set {
            m_moveInput = value;

            // When moveInput is set, we also set the direction so the actor's
            // sprite face accordingly.
            Vector3 newScale = transform.localScale;
            if (m_moveInput.x > 0.1f && newScale.x < 0  ||
                m_moveInput.x < -0.1f && newScale.x > 0) {
                newScale.x *= -1;
                transform.localScale = newScale;

            }
        }
    }


    // --- Unity Methods ---
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, $"object {name} doesn't have a RigidBody2D component");
    }

    protected virtual void OnValidate()
    {
        moveSpeed = m_moveSpeed;
    }

    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
