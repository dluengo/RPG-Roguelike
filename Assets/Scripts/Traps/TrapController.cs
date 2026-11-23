using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Collider2D))]
public class TrapController : MonoBehaviour
{
    // ---------- Members ----------
    [Header("TrapController Setup")]
    [SerializeField] protected AnimationClip m_activateClip;
    [SerializeField] protected AnimationClip m_deactivateClip;
    [SerializeField] protected AnimationClip m_idleClip;

    [SerializeField] int m_damage = 1;
    public int damage
    {
        get => m_damage;
        private set => m_damage = value;
    }

    [SerializeField] float m_cooldown = 1f;
    public float cooldown
    {
        get => m_cooldown;
        private set => m_cooldown = value;
    }

    [SerializeField] float m_knockbackForce = 15f;
    public float knockbackForce
    {
        get => m_knockbackForce;
        private set => m_knockbackForce = value;
    }

    [SerializeField] float m_knockbackDuration = 0.1f;
    public float knockbackDuration
    {
        get => m_knockbackDuration;
        private set => m_knockbackDuration = value;
    }

    protected Animator anim;
    //protected string m_playerTag = "Player";
    protected LayerMask m_playerLayer;

    private AnimatorOverrideController m_animOverrideController;
    private string m_activateAnimParamName = "Activate";
    private string m_activateClipName = "Activate-default";
    private string m_deactivateClipName = "Deactivate-default";
    private string m_idleClipName = "Idle-default";

    private bool m_onCooldown = false;


    // ---------- Events ----------
    public event Action OnActivate;


    // ---------- Unity Methods ----------
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        Debug.Assert(anim != null, $"object {name} doesn't have an Animator component");

        m_playerLayer = LayerMask.GetMask("Player");

        SetupAnimationClips();
    }

    protected virtual void OnValidate()
    {
        damage = m_damage;
        knockbackForce = m_knockbackForce;
        knockbackDuration = m_knockbackDuration;
        cooldown = m_cooldown;
    }


    // ---------- Collision Methods ----------
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_playerLayer) {
            ActivateTrap(collision);
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == m_playerLayer) {
            ActivateTrap(collision);
        }
    }


    // ---------- Public Methods ----------
    protected virtual void ActivateTrap(Collider2D collision)
    {
        if (m_onCooldown) {
            return;
        }

        anim.SetTrigger(m_activateAnimParamName);
        OnActivate?.Invoke();

        StartCoroutine(TrapCooldown());

        DoTrapLogic(collision);
    }

    protected virtual void DoTrapLogic(Collider2D collision)
    {
        ActorController actor = collision.GetComponent<ActorController>();
        if (actor != null) {
            if (damage > 0) {
                actor.TakeDamage(damage);
            }

            if (knockbackForce > 0f && knockbackDuration > 0f) {
                actor.Knockback(transform, knockbackForce, knockbackDuration);
            }
        }
    }


    // ---------- Helper Methods ----------
    private void SetupAnimationClips()
    {
        m_animOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = m_animOverrideController;

        // NOTE: https://docs.unity.com/en-us/engine/6000.5/script-reference/unityengine/animatoroverridecontroller/item
        //
        // this[string]:
        //
        // You should avoid calling this function more than once per frame per
        // Animator as each call will trigger a reallocation of the animator's
        // clip bindings. Instead use AnimatorOverrideController.ApplyOverrides.
        m_animOverrideController[m_activateClipName] = m_activateClip;
        m_animOverrideController[m_deactivateClipName] = m_deactivateClip;
        m_animOverrideController[m_idleClipName] = m_idleClip;
    }

    private IEnumerator TrapCooldown()
    {
        m_onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        m_onCooldown = false;
    }
}
