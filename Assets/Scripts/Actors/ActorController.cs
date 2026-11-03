using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ActorController : MonoBehaviour
{
    // ---------- Members ----------

    [SerializeField] protected float m_moveSpeed = 5f;
    public float moveSpeed {
        get => m_moveSpeed;
        private set => m_moveSpeed = value;
    }

    protected Rigidbody2D rb;
    protected Animator animator;

    [SerializeField] private Vector2 m_faceDir = Vector2.right;
    public Vector2 faceDir
    {
        get => m_faceDir;
        protected set {
            Debug.Log($"faceDir set to {value}");
            if (value.sqrMagnitude > m_moveThreshold) {
                m_faceDir = value.normalized;
            }
        }
    }

    // To be set by the class implementing ActorController. That class defines
    // how the actor moves, then this class handles the actual movement.
    private Vector2 m_moveInput = Vector2.zero;
    public Vector2 moveInput
    {
        get => m_moveInput;
        protected set {
            m_moveInput = Vector2.ClampMagnitude(value, 1f);

            UpdateDirection();
            UpdateAnimation();
        }
    }

    private float m_moveThreshold = 0.1f;


    // ---------- Unity Methods ----------

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, $"object {name} doesn't have a RigidBody2D component");

        animator = GetComponent<Animator>();
        Debug.Assert(animator != null, $"object {name} doesn't have an Animator component");
    }

    protected virtual void OnValidate()
    {
        moveSpeed = m_moveSpeed;
    }

    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }


    // ---------- Helper Methods ----------

    private void UpdateDirection()
    {
        if (moveInput.sqrMagnitude > m_moveThreshold) {
            faceDir = moveInput.normalized;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        animator.SetFloat("FaceX", faceDir.x);
        animator.SetFloat("FaceY", faceDir.y);

        if (moveInput.sqrMagnitude > m_moveThreshold) {
            animator.SetBool("IsMoving", true);
        } else {
            animator.SetBool("IsMoving", false);
        }
    }
}
