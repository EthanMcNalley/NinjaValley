using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInput : MonoBehaviour
{
    PlayerInput player_controls;
    PlayerMovement player_movement;

    public Vector2 movement_input;
    public Vector2 camera_input;

    public float vertical_input;
    public float horizontal_input;
    public float camera_horizontal_input;
    public float camera_vertical_input;

    public bool jump;
    public bool pause;


    private void OnEnable()
    {
        if (player_controls == null){
            player_controls = new PlayerInput();

            player_controls.PlayerMovement.Movement.performed += i => movement_input = i.ReadValue<Vector2>();
            player_controls.PlayerMovement.Camera.performed += i => camera_input = i.ReadValue<Vector2>();

            player_controls.PlayerActions.Jump.performed += i => jump = true;
            player_controls.PlayerActions.Jump.canceled += i => jump = false;

            player_controls.PlayerActions.Pause.performed += i => pause = true;
            player_controls.PlayerActions.Pause.canceled += i => pause = false;
            
        }

        player_controls.Enable();
    }

    public void HandleInputs(){
        HandleMovementInput();

        //HandleJumpingInput();
    }

    private void OnDisable()
    {
        player_controls.Disable();      
    }

    private void HandleMovementInput(){
        vertical_input = movement_input.y;
        horizontal_input = movement_input.x;

        camera_vertical_input = camera_input.y;
        camera_horizontal_input = camera_input.x;

    }

    /*private void HandleJumpingInput(){
        if (jump){
            jump = false;
            player_movement.HandleJumping();
        }
        
    }*/


}
