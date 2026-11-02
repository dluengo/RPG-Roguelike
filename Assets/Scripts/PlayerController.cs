using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // --- Members ---
    [SerializeField] private float m_moveSpeed = 5f;
    public float moveSpeed { get; private set; }

    protected Rigidbody2D rb;

    private Vector2 moveInput;


    // --- Unity Methods ---
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb != null, $"object {name} doesn't have a RigidBody2D component");
    }

    protected void OnValidate()
    {
        moveSpeed = m_moveSpeed;
    }

    protected void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }


    // --- Unity Input System Methods ---
    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
}
