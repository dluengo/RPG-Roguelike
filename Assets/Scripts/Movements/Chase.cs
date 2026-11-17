using System;
using UnityEngine;

public class Chase : Movement
{
    // ---------- Members ----------
    [Header("--- Chase Settings ---")]
    [SerializeField] protected Transform target;

    public override Vector2 direction { get => (target.position - transform.position).normalized; }


    // ---------- Unity Methods ----------
    protected override void Awake()
    {
        base.Awake();
        Debug.Assert(target != null, $"object {name} doesn't have a target assigned");
    }
}
