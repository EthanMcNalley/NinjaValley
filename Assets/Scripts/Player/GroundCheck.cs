using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform groundCheck;
    public float GroundCheckRadius;
    public LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, GroundCheckRadius, (int)groundLayer);
    }

    public bool IsGrounded => isGrounded;
}
