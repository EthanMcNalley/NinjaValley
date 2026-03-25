using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform groundCheck;
    public float GroundCheckRadius;
    public LayerMask[] groundLayer;
    public LayerMask child_ground_layer;
    [SerializeField] private bool isGrounded;
    public enum GroundType
    {
        WATER,
        GROUND,
        HARD
    }
    public GroundType ground_type;
    public GameObject parent_reset;
    private Vector3 starting_scale;

    void Start()
    {
        starting_scale = transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            ground_type = GroundType.WATER;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            ground_type = GroundType.GROUND;
        }
    }

    private void Update()
    {
        Collider[] child_ground = Physics.OverlapSphere(groundCheck.position, GroundCheckRadius, child_ground_layer, QueryTriggerInteraction.Ignore);
        if (child_ground.Length > 0)
        {
            transform.parent = child_ground[0].gameObject.transform.parent;
            // for (int i = 0; i < child_ground.Length; i++)
            // {
            //     if (child_ground[i])
            // }
        }

        else
        {
            transform.parent = parent_reset.transform;
            transform.parent = null;
            transform.localScale = starting_scale;
        }

        for (int i = 0; i < groundLayer.Length; i++)
        {
            Collider[] grounds = Physics.OverlapSphere(groundCheck.position, GroundCheckRadius, groundLayer[i], QueryTriggerInteraction.Ignore);
            isGrounded = grounds.Length > 0;

            if (isGrounded)
            {
                if (grounds[i].CompareTag("Hard"))
                {
                    ground_type = GroundType.HARD;
                }

                else if (grounds[i].CompareTag("Ground"))
                {
                    ground_type = GroundType.GROUND;
                }

                break;
            }   
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.darkBlue;
        Gizmos.DrawWireSphere(groundCheck.position, GroundCheckRadius);
    }
    public bool IsGrounded => isGrounded;
}
