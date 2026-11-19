using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MovementBase
{
    // ---------- Unity Input System Methods ----------
    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }
}
