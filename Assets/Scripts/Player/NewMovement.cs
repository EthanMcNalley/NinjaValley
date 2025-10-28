using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NewMovement : MonoBehaviour
{ 
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cam;
    private Vector2 inputVector;
    private Vector3 MoveDirection;
    
    
    private float rotationSpeed = 20f;

    [Header("Input Actions")]
    InputAction moveAction;
    InputAction jumpAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction dashAction;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cam = Camera.main.transform;

        if (InputSystem.actions)
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            lookAction = InputSystem.actions.FindAction("Look");
            runAction = InputSystem.actions.FindAction("Run");
            dashAction = InputSystem.actions.FindAction("Dash");
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        lookAction.Enable();
        runAction.Enable();  
        dashAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        lookAction.Disable();
        runAction.Disable();  
        dashAction.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        // Read input
        inputVector = moveAction.ReadValue<Vector2>();
        
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 move = (inputVector.x * camRight + inputVector.y * camForward).normalized;
        
        HandleRotation(move);
        //move = Vector3.ClampMagnitude(move, 1f);

        /*if (move != Vector3.zero)
        {
            transform.forward = move;
        }*/

        // Jump
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }
    
    private void HandleRotation(Vector3 moveDir){
        if (moveDir.sqrMagnitude < 0.0001f) return;
        
        moveDir.y = 0f;
        moveDir.Normalize();

        Quaternion target = Quaternion.LookRotation(moveDir);
        
        float t = 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
    }
}
