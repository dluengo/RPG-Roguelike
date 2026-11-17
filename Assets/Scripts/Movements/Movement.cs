using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    // --------- Members ----------
    [Header("--- Movement Settings ---")]
    [SerializeField] protected float speed;
    
    public abstract Vector2 direction { get; }
}
