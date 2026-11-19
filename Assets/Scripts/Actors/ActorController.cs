using System;
using System.Collections;
using UnityEngine;


public abstract class ActorController : MonoBehaviour
{
    // ---------- Members ----------
    [Header("--- ActorController Settings ---")]
    [SerializeField] private Vector2 m_faceDir = Vector2.right;
    public Vector2 faceDir
    {
        get => m_faceDir;
        set {
            if (value.sqrMagnitude > 0.1f) {
                m_faceDir = value.normalized;
                CheckAndFlipDir();
            }
        }
    }

    [SerializeField] protected GameObject m_popupPrefab;
    [SerializeField] protected MovementBase movement;
    [SerializeField] protected AttackBase attack;

    // NOTE: This property has a variety of values. It represents the layer
    // where the enemies of this ActorController are. Enemy for the player
    // and Player for the enemies.
    [SerializeField] protected LayerMask m_enemyLayer;
    public LayerMask enemyLayer
    {
        get => m_enemyLayer;
        protected set => m_enemyLayer = value;
    }

    protected Rigidbody2D rb;
    protected Animator animator;
    protected HealthController health;
    private bool m_canMove = true;
    protected bool canMove
    {
        get => m_canMove;
        set {
            m_canMove = value;
            if (!m_canMove) {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
    protected float m_dmgPopupDuration = 0.3f;

    private string m_isMovingAnimParamName = "IsMoving";
    private bool m_isMoving = false;
    public bool isMoving
    {
        get => m_isMoving;
        protected set {
            // We are actually switching.
            if (value != m_isMoving) {
                m_isMoving = value;
                animator.SetBool(m_isMovingAnimParamName, value);
            }
        }
    }

    private string m_isAttackingAnimParamName = "IsAttacking";
    private bool m_isAttacking = false;
    public bool isAttacking
    {
        get => m_isAttacking;
        protected set {
            // We are actually switching.
            if (value != m_isAttacking) {
                m_isAttacking = value;
                animator.SetBool(m_isAttackingAnimParamName, value);
            }
        }
    }

    private AnimatorOverrideController m_animOverrideController = null;
    private AttackAnimBehaviour attackAnimSMB;


    // ---------- Events ----------
    public event Action OnAttackAnimationEnd;


    // ---------- Unity Methods ----------

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, $"object {name} doesn't have a RigidBody2D component");

        animator = GetComponent<Animator>();
        Debug.Assert(animator != null, $"object {name} doesn't have an Animator component");

        // NOTE: Actors are allowed to not have a HealthController.
        health = GetComponent<HealthController>();

        // NOTE: Actors are allowed to not have a MovementBase component, they won't move.
        //movement = GetComponent<MovementBase>();

        // NOTE: Actors are allowed to not have a AttackBase component, they won't attack.
        //attack = GetComponent<AttackBase>();

        if (animator != null) {
            m_animOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = m_animOverrideController;

            attackAnimSMB = animator.GetBehaviour<AttackAnimBehaviour>();
        }
    }

    protected virtual void OnEnable()
    {
        if (health != null) {
            health.OnDamageTaken += DamageTakenHandler;
        }

        if (attackAnimSMB != null) {
            attackAnimSMB.OnAttackAnimEnd += AttackAnimEndHandler;
        }
    }

    protected virtual void OnDisable()
    {
        if (health != null) {
            health.OnDamageTaken -= DamageTakenHandler;
        }

        if (attackAnimSMB != null) {
            attackAnimSMB.OnAttackAnimEnd -= AttackAnimEndHandler;
        }
    }

    protected virtual void OnValidate()
    {
        enemyLayer = m_enemyLayer;
    }


    // ---------- MOVEMENT ----------
    protected virtual void FixedUpdate()
    {
        if (canMove) {
            if (movement != null) {
                if (movement.speed > 0) {
                    if (movement.direction.sqrMagnitude > movement.moveThreshold) {
                        if (canMove) {
                            isMoving = true;
                            rb.linearVelocity = movement.direction * movement.speed;
                            UpdateDirection();
                            return;
                        }
                    }
                    else {
                        // No movement input.
                        isMoving = false;
                        rb.linearVelocity = Vector2.zero;
                    }
                }
            }

            isMoving = false;
        }
    }


    // ---------- Public Methods ----------

    public void SetMovementEnable(bool value)
    {
        canMove = value;
    }

    public void ConfigureAttack(AnimationClip attackAnimClip, string currClipName)
    {
        if (m_animOverrideController != null) {
            m_animOverrideController[currClipName] = attackAnimClip;
        }
    }

    public void Attack()
    {
        isAttacking = true;
    }

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
        if (movement.direction.sqrMagnitude > movement.moveThreshold) {
            faceDir = movement.direction.normalized;

            CheckAndFlipDir();
        }
    }

    //private void UpdateAnimation()
    //{
    //    animator.SetFloat("MoveX", movement.direction.x);
    //    animator.SetFloat("MoveY", movement.direction.y);

    //    animator.SetFloat("FaceX", faceDir.x);
    //    animator.SetFloat("FaceY", faceDir.y);

    //    if (movement.direction.sqrMagnitude > movement.moveThreshold) {
    //        animator.SetBool("IsMoving", true);
    //    } else {
    //        animator.SetBool("IsMoving", false);
    //    }
    //}

    private IEnumerator KnockbackCR(Transform origin, float force, float duration)
    {
        canMove = false;

        Vector2 knockbackDir = (transform.position - origin.position).normalized;
        rb.linearVelocity = knockbackDir * force;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        canMove = true;
    }

    private void CheckAndFlipDir()
    {
        Vector3 newScale = transform.localScale;
        if (faceDir.x < -0.1f && newScale.x > 0 ||
            faceDir.x > 0.1f && newScale.x < 0) {
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }


    // ---------- Event Handlers ---------- 
    private void AttackAnimEndHandler()
    {
        isAttacking = false;
        OnAttackAnimationEnd?.Invoke();
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
