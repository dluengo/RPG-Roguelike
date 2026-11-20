using System;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public abstract class AttackBase : MonoBehaviour
{
    // ---------- Members ----------
    [Header("--- AttackBase Settings ---")]
    [SerializeField] protected AnimationClip attackAnimationClip;
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

    protected bool m_attackOnCooldown = false;
    public bool attackOnCooldown
    {
        get => m_attackOnCooldown;
        protected set => m_attackOnCooldown = value;
    }

    protected ActorController actor;


    // ---------- Events ---------- 
    public event Action OnAttackEnd;


    // ---------- Unity Methods ----------
    protected virtual void Awake()
    {
        actor = GetComponent<ActorController>();
        Debug.Assert(actor != null, $"object {name} doesn't have an ActorController component");

        Debug.Assert(attackAnimationClip != null, $"object {name} doesn't have an AnimationClip component");
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
    }


    // ---------- Animation Event Methods ---------- 
    public virtual bool StartAttack()
    {
        if (!actor.isAttacking && !attackOnCooldown) {
            actor.Attack();

            StartCoroutine(AttackCooldownCR(cooldown));

            return true;
        }

        return false;
    }

    // NOTE: The animation clip SHOULD bind an animation event to this method
    public abstract void TryHit();

    public virtual void EndAttack()
    {
        OnAttackEnd?.Invoke();
    }


    // ---------- Coroutines ----------
    protected IEnumerator AttackCooldownCR(float cooldown)
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        attackOnCooldown = false;
    }

    // ---------- Event Handlers ---------- 
    protected virtual void AttackAnimationEndHandler()
    {
        EndAttack();
    }
}
