using System;
using System.Reflection.Metadata.Ecma335;
using Unity.VisualScripting;
using UnityEngine;

public class Chase : MovementBase
{
    // ---------- Members ---------- 
    [Header("--- Chase Settings ---")]
    [SerializeField] protected Transform target;

    [SerializeField] protected float minDistance = 0.75f;


    // ---------- Unity Methods ---------- 
    protected virtual void Update()
    {
        Vector2 dir = target.position - transform.position;
        Vector2 dirNormalized = dir.normalized;

        if (dir.sqrMagnitude > minDistance) {
            direction = dirNormalized;
        }
        else {
            direction = Vector2.zero;
        }
    }
}
