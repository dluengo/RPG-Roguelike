using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public abstract class ActorController : MonoBehaviour
{
    // ---------- Members ----------
    [Header("--- ActorController Settings ---")]
    [SerializeField] protected float m_moveSpeed = 5f;
    public float moveSpeed {
        get => m_moveSpeed;
        private set => m_moveSpeed = value;
    }

    [SerializeField] private Vector2 m_faceDir = Vector2.right;
    public Vector2 faceDir
    {
        get => m_faceDir;
        protected set {
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

    [SerializeField] protected GameObject m_popupPrefab;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected HealthController health;
    protected float m_moveThreshold = 0.1f;
    protected bool canMove = true;
    protected float m_dmgPopupDuration = 0.3f;


    // ---------- Unity Methods ----------

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, $"object {name} doesn't have a RigidBody2D component");

        animator = GetComponent<Animator>();
        Debug.Assert(animator != null, $"object {name} doesn't have an Animator component");

        // NOTE: Actors are allowed to not have a HealthController.
        health = GetComponent<HealthController>();
    }

    protected virtual void OnEnable()
    {
        if (health != null) {
            health.OnDamageTaken += DamageTakenHandler;
        }
    }

    protected virtual void OnDisable()
    {
        if (health != null) {
            health.OnDamageTaken -= DamageTakenHandler;
        }
    }

    protected virtual void OnValidate()
    {
        moveSpeed = m_moveSpeed;
    }

    protected virtual void FixedUpdate()
    {
        if (canMove) {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }


    // ---------- Public Methods ----------

    public void TakeDamage(int damage)
    {
        if (health != null) {
            health.TakeDamage(damage);
        }
    }

    public void Knockback(Transform origin, float force, float duration)
    {
        StartCoroutine(KnockbackCR(origin, force, duration));
    }


    // ---------- Helper Methods ----------

    private void UpdateDirection()
    {
        if (moveInput.sqrMagnitude > m_moveThreshold) {
            faceDir = moveInput.normalized;

            Vector3 newScale = transform.localScale;
            if (faceDir.x < -m_moveThreshold && newScale.x > 0 ||
                faceDir.x > m_moveThreshold && newScale.x < 0) {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
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

    private IEnumerator KnockbackCR(Transform origin, float force, float duration)
    {
        canMove = false;

        Vector2 knockbackDir = (transform.position - origin.position).normalized;
        rb.linearVelocity = knockbackDir * force;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        canMove = true;
    }

    private void DamageTakenHandler(int damage)
    {
        animator.SetTrigger("TurnRed");

        GameObject popupGO = Instantiate(m_popupPrefab, transform.position, Quaternion.identity);
        if (popupGO != null) {

            PopupController popup = popupGO.GetComponent<PopupController>();
            if (popup != null) {
                popup.Setup(m_dmgPopupDuration, damage.ToString());
            }
        }
    }
}
