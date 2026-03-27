using System;
using FMOD.Studio;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public NewMovement playerMovement;
    public Transform groundCheck;
    public float GroundCheckRadius;
    public LayerMask[] groundLayer;
    public LayerMask child_ground_layer;
    [SerializeField] private bool isGrounded;
    public enum GroundType
    {
        HARD = 0,
        GROUND = 1,
        WATER = 2
    }
    public GroundType ground_type;
    public GameObject parent_reset;
    private Vector3 starting_scale;

    void Start()
    {
        starting_scale = transform.localScale;
        playerMovement = GetComponent<NewMovement>();
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

        isGrounded = false;
        Collider groundedOn = null;

        for (int i = 0; i < groundLayer.Length; i++)
        {
            Collider[] grounds = Physics.OverlapSphere(groundCheck.position, GroundCheckRadius, groundLayer[i], QueryTriggerInteraction.Ignore);

            if (grounds.Length > 0)
            {
                isGrounded = true;
                groundedOn = grounds[0]; //Ethan this is 0 because it checks for the first ground tag it collides, but it's not we would have 2 anyways
                break;
            }
        }
        
        //pulled this out, footstep sound stuff
        if (isGrounded && playerMovement.IsMoving())
        {
            PLAYBACK_STATE state;
            AudioManager.instance.footstepEventInstance.getPlaybackState(out state);
            if (state == PLAYBACK_STATE.STOPPED)
            {
                AudioManager.instance.footstepEventInstance.start();
            }

            if (groundedOn.CompareTag("Hard") && ground_type != GroundType.WATER)
            {
                ground_type = GroundType.HARD;
            }
            else if (ground_type != GroundType.WATER)
            {
                ground_type = GroundType.GROUND;
            }

            AudioManager.instance.SetFootstepArea(ground_type);

            if (playerMovement.IsRunning())
            {
                AudioManager.instance.SetFootstepRunning(1);
            }
            else
            {
                AudioManager.instance.SetFootstepRunning(0);
            }
        }
        else
        {
            AudioManager.instance.footstepEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.darkBlue;
        Gizmos.DrawWireSphere(groundCheck.position, GroundCheckRadius);
    }
    public bool IsGrounded => isGrounded;
}
