using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform groundCheck;
    public float GroundCheckRadius;
    public LayerMask[] groundLayer;
    [SerializeField] private bool isGrounded;
    
    private void Update()
    {
        for (int i = 0; i < groundLayer.Length; i++)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, GroundCheckRadius, (int)groundLayer[i]);
            if (isGrounded)
            {
                break;
            }   
        }
    }

    public bool IsGrounded => isGrounded;
}
