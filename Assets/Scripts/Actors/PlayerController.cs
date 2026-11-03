using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    // ---------- Unity Input System Methods ----------

    public void OnMove(InputValue input)
    {
        Debug.Log($"Move input received: {input.Get<Vector2>()}");
        moveInput = input.Get<Vector2>();
    }
}
