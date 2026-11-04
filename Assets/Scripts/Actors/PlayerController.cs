using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    // ---------- Unity Input System Methods ----------

    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
}
