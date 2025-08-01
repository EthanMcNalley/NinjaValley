using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    MovementInput movement_input;
    Vector3 move_direction;
    Transform cam;
    Rigidbody rb;
    public float acceleration = 7.0f;
    public float max_speed = 7.0f;
    public float rotation_speed = 15.0f;

    public bool is_jumping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement_input = GetComponent<MovementInput>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleAllMovement(){
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement(){
        move_direction = cam.forward * movement_input.vertical_input;
        move_direction = move_direction + cam.right * movement_input.horizontal_input;
        move_direction.Normalize();
        move_direction.y = 0;
        move_direction = move_direction * max_speed;

        Vector3 velocity = move_direction;
        // rb.linearVelocity = velocity;

        //Max speed check
        if (rb.linearVelocity.x > max_speed){
            rb.linearVelocity = new Vector3 (max_speed, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        else if (rb.linearVelocity.x < -max_speed){
            rb.linearVelocity = new Vector3 (-max_speed, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        if (rb.linearVelocity.z > max_speed){
            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, rb.linearVelocity.y, max_speed);
        }

        else if (rb.linearVelocity.z < -max_speed){
            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, rb.linearVelocity.y, -max_speed);
        }

        rb.AddForce(velocity * acceleration);

    }

    private void HandleRotation(){
        Vector3 target_direction = Vector3.zero;

        target_direction = cam.forward * movement_input.vertical_input;
        target_direction = target_direction + cam.right * movement_input.horizontal_input;
        target_direction.Normalize();
        target_direction.y = 0;

        if (target_direction == Vector3.zero){
            target_direction = transform.forward;
        }

        Quaternion target_rotation = Quaternion.LookRotation(target_direction);
        Quaternion player_rotation = Quaternion.Slerp(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);

        transform.rotation = player_rotation; 
    }

    public void HandleJumping(){
        if (is_jumping){
            
        }
    }
}
