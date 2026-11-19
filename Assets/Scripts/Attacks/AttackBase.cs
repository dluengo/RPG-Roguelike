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

    protected ActorController actor;


    // ---------- Unity Methods ----------
    protected virtual void Awake()
    {
        actor = GetComponent<ActorController>();
        Debug.Assert(actor != null, $"object {name} doesn't have an ActorController component");

        Debug.Assert(attackAnimationClip != null, $"object {name} doesn't have an AnimationClip component");
    }

    protected void OnValidate()
    {
        damage = m_damage;
    }


    // ---------- Animation Event Methods ---------- 
    // NOTE: The attack animation clip SHOULD have an animation event bound
    // to this method.
    //public abstract void ThrowAttack();
    public abstract void StartAttack();
    public abstract void TryHit();
    public abstract void EndAttack();
}
