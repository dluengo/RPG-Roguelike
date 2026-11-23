using System;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public abstract class AttackBase : MonoBehaviour
{
    // ---------- Members ----------
    [Header("--- AttackBase Settings ---")]
    [SerializeField] protected AnimationClip m_animationClip;
    public AnimationClip animationClip
    {
        get => m_animationClip;
        protected set => m_animationClip = value;
    }
    [SerializeField] protected int m_damage;
    public int damage
    {
        get => m_damage;
        protected set => m_damage = value;
    }

    [SerializeField] protected float m_cooldown = 1;
    public float cooldown
    {
        get => m_cooldown;
        protected set => m_cooldown = value;
    }

    //// NOTE: When range is 0 internally we decide a value for it.
    //[SerializeField] protected float m_range = 0;
    //public float range
    //{
    //    get => m_range;
    //    protected set => m_range = value;
    //}

    protected bool m_onCooldown = false;
    public bool onCooldown
    {
        get => m_onCooldown;
        protected set => m_onCooldown = value;
    }

    protected ActorController actor;

    protected float knockbackForce = 15;
    protected float knockbackDuration = 0.1f;

    protected Transform target = null;


    // ---------- Events ---------- 
    public event Action<Collider2D> OnAttackHit;
    public event Action OnAttackEnd;


    // ---------- Unity Methods ----------
    protected virtual void Awake()
    {
        actor = GetComponent<ActorController>();
        Debug.Assert(actor != null, $"object {name} doesn't have an ActorController component");

        Debug.Assert(animationClip != null, $"object {name} doesn't have an AnimationClip component");
    }

    protected virtual void OnEnable()
    {
        actor.OnAttackAnimationEnd += AttackAnimationEndHandler;
    }

    protected virtual void OnDisable()
    {
        actor.OnAttackAnimationEnd -= AttackAnimationEndHandler;
    }

    protected void OnValidate()
    {
        damage = m_damage;
        cooldown = m_cooldown;
        //range = m_range;
    }


    // ---------- Animation Event Methods ---------- 
    public bool StartAttack()
    {
        return StartAttack(actor.faceDir);
    }

    public virtual bool StartAttack(Vector2 direction)
    {
        if (!actor.isAttacking && !onCooldown) {
            //Debug.Log($"obj {name} starts attacking in direction {direction}, frame {Time.frameCount}");
            actor.Attack(direction.normalized);

            StartCoroutine(AttackCooldownCR(cooldown));

            return true;
        }

        return false;
    }

    public virtual bool StartAttack(Transform target)
    {
        Vector2 attackDir = (target.position - transform.position).normalized;
        bool canAttack = StartAttack(attackDir);
        this.target = canAttack ? target : null;
        return canAttack;
    }

    public virtual void EndAttack()
    {
        //Debug.Log($"obj {name} ends attack, frame {Time.frameCount}");
        target = null;
        OnAttackEnd?.Invoke();
    }


    // ---------- Coroutines ----------
    protected IEnumerator AttackCooldownCR(float cooldown)
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    // ---------- Event Handlers ---------- 
    protected virtual void AttackAnimationEndHandler()
    {
        EndAttack();
    }
}
