using System;
using System.Reflection.Metadata.Ecma335;
using Unity.VisualScripting;
using UnityEngine;

public class Chase : MovementBase
{
    // ---------- Members ---------- 
    public Transform target;


    // ---------- Unity Methods ---------- 
    protected virtual void Update()
    {
        direction = (target.position - transform.position).normalized;
    }
}
