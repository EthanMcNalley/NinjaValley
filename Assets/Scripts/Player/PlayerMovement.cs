using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    //MovementInput movement_input;
    PlayerInput player_controls;
    Vector3 move_direction;
    Transform cam;
    Rigidbody rb;
    public float acceleration = 7.0f;
    public float max_speed = 7.0f;
    public float run_Speed = 20.0f;
    public float rotation_speed = 15.0f;
    public float dash_speed = 0f, dash_time = 0.05f;
    float normal_speed;
    float normal_acceleration;
    float normal_roto_speed;
    public float jump_power = 16.0f;

    private Vector2 moveValue;
    private Vector2 lookValue;

    public bool isGrounded = true;

    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction dashAction;
    private Animator animator;

    public GroundCheck ground_check;

    //Audio Stuff
    [Header("Sound Stuff")]
    public EventReference JumpSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //movement_input = GetComponent<MovementInput>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        normal_speed = max_speed;
        normal_acceleration = acceleration;
        normal_roto_speed = rotation_speed;
        animator = GetComponent<Animator>();


        if (InputSystem.actions)
        {
            jumpAction = InputSystem.actions.FindAction("Jump");
            moveAction = InputSystem.actions.FindAction("Move");
            lookAction = InputSystem.actions.FindAction("Look");
            runAction = InputSystem.actions.FindAction("Run");
            dashAction = InputSystem.actions.FindAction("Dash");
        }
    }
    // Update is called once per frame
    void Update()
    {
        // if (TimeManager.time_state == TimeManager.TimeState.SLOWED){
        //     max_speed = normal_speed/Time.timeScale;
        //     acceleration = normal_acceleration/Time.timeScale;
        //     rotation_speed = normal_roto_speed/Time.timeScale;
        // }

        // else{
        //     max_speed = normal_speed;
        //     acceleration = normal_acceleration;
        //     rotation_speed = normal_roto_speed;
        // }

        moveValue = moveAction.ReadValue<Vector2>();
        lookValue = lookAction.ReadValue<Vector2>();

        if ((moveValue != Vector2.zero) && (rb.linearVelocity.y < 0.01f && rb.linearVelocity.y > -0.01f))
        {
            animator.SetBool("Moving", true);
        }

        else
        {
            animator.SetBool("Moving", false);
        }


        if (isGrounded)
        {
            if (jumpAction.triggered)
            {
                Jump();
                animator.SetTrigger("Jump");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack1");
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        rb.AddForce(Vector3.up * jump_power, ForceMode.Impulse);

        if (JumpSound.Path.Length > 0)
            AudioManager.instance.PlayOneShot(JumpSound, transform.position);
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dash_time)
        {
            rb.AddForce(Vector3.forward * dash_speed, ForceMode.Impulse);
            yield return null;
        }
    }
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        move_direction = cam.forward * moveValue.y;
        move_direction = move_direction + cam.right * moveValue.x;
        move_direction.Normalize();
        move_direction.y = 0;
        // if (runAction.ReadValue<float>() > 0f)
        // {
        //     max_speed = run_Speed;
        //     Debug.Log(runAction.ReadValue<float>());
        // }
        // else{
        //     max_speed = 7.0f;
        // }


        move_direction = move_direction * max_speed;

        Vector3 velocity = move_direction;
        // rb.linearVelocity = velocity;

        //Max speed check
        if (rb.linearVelocity.x > max_speed)
        {
            rb.linearVelocity = new Vector3(max_speed, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        else if (rb.linearVelocity.x < -max_speed)
        {
            rb.linearVelocity = new Vector3(-max_speed, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        if (rb.linearVelocity.z > max_speed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, max_speed);
        }

        else if (rb.linearVelocity.z < -max_speed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, -max_speed);
        }

        Vector3 newVel = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, newVel, acceleration * Time.deltaTime);

    }

    private void HandleRotation()
    {
        Vector3 target_direction = Vector3.zero;

        target_direction = cam.forward * moveValue.y;
        target_direction = target_direction + cam.right * moveValue.x;
        target_direction.Normalize();
        target_direction.y = 0;

        if (target_direction == Vector3.zero)
        {
            target_direction = transform.forward;
        }

        Quaternion target_rotation = Quaternion.LookRotation(target_direction);
        Quaternion player_rotation = Quaternion.Slerp(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);

        transform.rotation = player_rotation;
    }

    /*public void HandleJumping(){
        if (is_jumping){
            
        }
    }*/

    private void FixedUpdate()
    {
        //isGrounded = ground_check.isgrounded;
    }

    private void OnDrawGizmos()
    {
    }
}
