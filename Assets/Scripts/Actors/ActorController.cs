using NUnit.Framework;
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

    private string m_isShootingAnimParamName = "IsShooting";
    private bool m_isShooting = false;
    public bool isShooting
    {
        get => m_isShooting;
        protected set {
            if (value != m_isShooting) {
                // NOTE: Doesn't trigger SetBool() in m_isAttacking
                m_isShooting = m_isAttacking = value;
                
                animator.SetBool(m_isShootingAnimParamName, value);
            }
        }
    }

    private Vector2 m_attackDir = Vector2.right;
    public Vector2 attackDir
    {
        get => m_attackDir;
        protected set => m_attackDir = value;
    }

    private AnimatorOverrideController animOverrideController = null;
    //private AttackAnimBehaviour attackAnimSMB;
    //private string defaultAttackAnimClipName = "Attack";
    //private string currentAttackAnimClipName;


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
            animOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animOverrideController;
        }

        //if (attack != null) {
        //    ConfigureAttack(attack.animationClip, defaultAttackAnimClipName);
        //}
    }

    protected virtual void OnEnable()
    {
        if (health != null) {
            health.OnDamageTaken += DamageTakenHandler;
        }

        foreach (AttackAnimBehaviour behav in animator.GetBehaviours<AttackAnimBehaviour>()) {
            behav.OnAttackAnimEnd += AttackAnimEndHandler;
        }

        //AttackAnimBehaviour attackAnimSMB = animator.GetBehaviour<AttackAnimBehaviour>();
        //if (attackAnimSMB != null) {
        //    attackAnimSMB.OnAttackAnimEnd += AttackAnimEndHandler;
        //}
    }

    protected virtual void OnDisable()
    {
        if (health != null) {
            health.OnDamageTaken -= DamageTakenHandler;
        }

        foreach (AttackAnimBehaviour behav in animator.GetBehaviours<AttackAnimBehaviour>()) {
            behav.OnAttackAnimEnd -= AttackAnimEndHandler;
        }

        //AttackAnimBehaviour attackAnimSMB = animator.GetBehaviour<AttackAnimBehaviour>();
        //if (attackAnimSMB != null) {
        //    attackAnimSMB.OnAttackAnimEnd -= AttackAnimEndHandler;
        //}
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
                        isMoving = true;
                        rb.linearVelocity = movement.direction * movement.speed;
                        UpdateDirection();
                        return;
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
        if (value != canMove) {
            //Debug.Log($"obj {name} switching canMove to {value}");
        }
        canMove = value;
    }

    public void Attack()
    {
        Attack(faceDir);
    }

    public void Attack(Vector2 direction)
    {
        attackDir = direction.normalized;
        faceDir = attackDir;
        
        if (attack is RangeAttack) {
            isShooting = true;
        }
        else {
            isAttacking = true;
        }
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

    private void CheckAndFlipDir()
    {
        Vector3 newScale = transform.localScale;
        if (faceDir.x < -0.1f && newScale.x > 0 ||
            faceDir.x > 0.1f && newScale.x < 0) {
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    //private void ConfigureAttack(AnimationClip attackAnimClip, string currClipName)
    //{
    //    if (animOverrideController != null) {
    //        animOverrideController[currClipName] = attackAnimClip;
    //        currentAttackAnimClipName = attackAnimClip.name;
    //    }
    //}


    // ---------- Coroutines ----------
    private IEnumerator KnockbackCR(Transform origin, float force, float duration)
    {
        canMove = false;

        Vector2 knockbackDir = (transform.position - origin.position).normalized;
        rb.linearVelocity = knockbackDir * force;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        canMove = true;
    }


    // ---------- Event Handlers ---------- 
    private void AttackAnimEndHandler()
    {
        if (isShooting) {
            isShooting = false;
        }
        else {
            isAttacking = false;
        }

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
