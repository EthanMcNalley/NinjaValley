using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Transform camera;
    
    //input stuff
    private InputAction jumpAction;  
    private InputAction moveAction;  
    private InputAction lookAction;  
    private InputAction runAction;   
    private InputAction dashAction;  
    
    private Vector2 lookValue;
    
    private Animator animator;
    private CharacterController characterController;
    
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    
    private Vector3 playerVelocity;
    [SerializeField]
    private bool groundedPlayer;
    private bool firstLand;
    private bool jump;
    
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        camera = Camera.main.transform;
        
        //Input
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
        lookValue = lookAction.ReadValue<Vector2>();
        
        groundedPlayer = characterController.isGrounded;
        
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Read input
        Vector2 input = moveAction.ReadValue<Vector2>();
        
        Vector3 cameraForward = camera.forward;
        Vector3 cameraRight = camera.right;
        
        //to ignore angles stuff
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 move = (cameraForward * input.y + cameraRight * input.x).normalized;


        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        if (jumpAction.triggered)
        {
            jump = true;
        }
        
        if (jump && groundedPlayer)
        {
            jump = false;
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            animator.SetTrigger("Jump");
        }
        
        AnimateCharacter(move);
        
        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
        characterController.Move(finalMove * Time.deltaTime);
    }

    private void AnimateCharacter(Vector3 move)
    {
        if (move != Vector3.zero && groundedPlayer)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
}
