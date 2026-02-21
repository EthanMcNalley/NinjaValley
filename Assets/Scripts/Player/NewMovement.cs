using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NewMovement : MonoBehaviour
{ 
    public float playerWalkSpeed = 20.0f;
    public float playerRunSpeed = 30.0f;
    [SerializeField]private float playerCurrSpeed = 20.0f;
    public float gravityValue = -9.81f;

    private CharacterController controller;
    private GroundCheck groundCheck;
    private LockIn lockIn;
    private Vector3 playerVelocity;
    [SerializeField]private bool groundedPlayer;
    public float coyote_time_amount = 0.25f;
    private float coyote_timer = 0;
    private Transform cam;
    private Vector2 inputVector;
    private Vector3 moveDirection;
    private Animator animator;
    public float rotationSpeed = 20f;
    public bool translationDisabled = false;
    public float max_fall_speed = -40.0f;
    
    [Header("Knockback")]
    private Vector3 knockbackForce;
    [SerializeField]private float knockbackDecay = 10f;
    [SerializeField]private const float defaultKnockbackTime = 0.5f;
    private float knockbackTimer;
    private bool knocked_back = false;
    
    [Header("Jump")]
    public float minJumpHeight = 0.5f;
    public float maxJumpHeight = 1.5f;
    public float timeToMaxHeight = 0.5f;
    
    private float baseGravity;
    private float initialJumpVelocity;
    private float minJumpCutVelocity;
    
    public bool lockedIn;

    [Header("Input Actions")]
    InputAction moveAction;
    InputAction jumpAction;
    private InputAction lookAction;
    public bool canMove = true;
    
    private bool afterDodge;

    public enum moveState
    {
        Idle,
        Walking,
        Running,
        Dodging,
        Jumping
    }
    [SerializeField]private moveState currentState = moveState.Idle;
    private moveState prevState;
    public bool double_jump_able = false;
    public bool double_jump = false;
    public GameObject moving_particle;
    public GameObject landing_particle;
    public GameObject dash_particle;
    private bool play_landing = false;
    private Vector3 hit_normal;
    public float slide_friction;
    private UIManager UI_manager;
    public static bool time_able = true;

    private void Awake()
    {
        UI_manager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        coyote_timer = coyote_time_amount;
        groundCheck = GetComponent<GroundCheck>();
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        lockIn = GetComponent<LockIn>();
        cam = Camera.main.transform;
        currentState = moveState.Idle;

        if (InputSystem.actions)
        {
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            lookAction = InputSystem.actions.FindAction("Look");
        }
        
        //Like wtf physics is this, just found it online
        baseGravity = -(2f * maxJumpHeight) / (timeToMaxHeight * timeToMaxHeight);
        initialJumpVelocity = (2f * maxJumpHeight) / timeToMaxHeight;
        
        minJumpCutVelocity = Mathf.Sqrt(2f * Mathf.Abs(baseGravity) * minJumpHeight);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        lookAction.Enable();
        
        if (jumpAction != null)
        {
            jumpAction.performed += OnJumpPerformed;
            jumpAction.canceled  += OnJumpCanceled;
        }
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        lookAction.Disable();
        
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJumpPerformed;
            jumpAction.canceled  -= OnJumpCanceled;
        }
    }

    public void EnableMovement()
    {
        if (canMove) return;
        canMove =  true;
    }
    
    public void DisableMovement()
    {
        if (!canMove) return;
        canMove =  false;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            Jump();
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if (playerVelocity.y > 0f)
        { 
            playerVelocity.y = Mathf.Min(playerVelocity.y, minJumpCutVelocity);
        }
        
    }

    public void DodgeStart()
    {
        //SetTranslationDisabled(true);
        SetTranslationDisabled(true);
        SetNewMoveState(moveState.Dodging);
    }
    
    public void DodgeEnd()
    {
       afterDodge = true;
       SetTranslationDisabled(false);
       EnableMovement();
    }

    void Update()
    {
        groundedPlayer = groundCheck.IsGrounded;

        if (!play_landing && groundedPlayer)
        {
            Instantiate(landing_particle, transform.position, Quaternion.Euler(-90, 0, 0));
        } 

        moveDirection = translationDisabled ? Vector3.zero : GetInputVector();

        if (!lockIn.IsLockOn())
        {
            HandleRotation(moveDirection);
        }
        else //comment out this else for a funny Tengu fly up thing for the player lol, the whole model will flip
        {
            Vector3 direction = lockIn.target.transform.position - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);   
        }

        animator.SetBool("Moving", moveDirection.magnitude > 0.01f & groundedPlayer);

        // Apply gravity, also means the player is Idle
        if (groundedPlayer && playerVelocity.y <= 0f)
        {
            play_landing = true;
            playerVelocity.y = -2f;
            double_jump = true;
            coyote_timer = 0f;
        }

        else {
            if (coyote_timer < coyote_time_amount)
            {
                coyote_timer = coyote_timer + Time.deltaTime;
            }
        }

        if (currentState != moveState.Dodging && groundedPlayer && !translationDisabled)
        {
            if (moveDirection.magnitude >= 0.01f)
            {
                // Decide run vs walk:
                if (afterDodge)
                {
                    animator.SetBool("Moving", true);
                    currentState = moveState.Running;
                }
                else
                {
                    animator.SetBool("Moving", true);
                    currentState = moveState.Walking;
                }
            }
            else
            {
                animator.SetBool("Moving", false);
                currentState = moveState.Idle;
            
                // Once we stop moving and go Idle, clear the forced-run state
                afterDodge = false;
            }
        }

        if (currentState != prevState)
        {
            StateChanged();
            prevState = currentState;
        }

        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }

        //Max Fall Speed
        if (playerVelocity.y < max_fall_speed)
        {
            playerVelocity.y = max_fall_speed;
        }
        
        //knockback stuff
        if (knockbackTimer < 0){
            if (knocked_back){
                EnableMovement();
                knocked_back = false;
            } 
        }

        else{
            knocked_back = true;
            knockbackTimer -=  Time.deltaTime;
            DisableMovement();
        }
        
        knockbackForce = Vector3.Lerp(knockbackForce, Vector3.zero, knockbackDecay * Time.deltaTime);
        if (knockbackForce.magnitude <= 0.1f && knockbackForce != Vector3.zero)
        {
            knockbackForce = Vector3.zero;
        }
        
        
        if (controller.slopeLimit < Vector3.Angle(hit_normal, Vector3.up) && !groundedPlayer) {
            moveDirection.x += (1f - hit_normal.y) * hit_normal.x * (1f - slide_friction);
            moveDirection.z += (1f - hit_normal.y) * hit_normal.z * (1f - slide_friction);
        }
        
        // Combine horizontal and vertical movement
        Vector3 horizontal = (playerCurrSpeed * moveDirection);
        Vector3 vertical = (playerVelocity.y * Vector3.up);
        
        if ((controller.collisionFlags & CollisionFlags.Below) != 0 &&
            (controller.collisionFlags & CollisionFlags.Sides) != 0 &&
            !groundedPlayer)
        {
            //stuckRestrict = 0.1f;
            Vector3 wallPushDir = hit_normal;
            wallPushDir.y = 0f;
            wallPushDir.Normalize();

            horizontal = wallPushDir * 11f;
            
            //horizontal = horizontal.normalized * 5f;
            Debug.Log("Restricting");
        }
        
        controller.Move(Time.deltaTime * (horizontal + vertical) + knockbackForce);
        
        if (!groundedPlayer){
            animator.SetFloat("YVelocity", vertical.y);
        }

        else
        {
            animator.SetFloat("YVelocity", 0);
        }
    }


    private void Jump()
    {
        play_landing = false;
        
        //Normal Jump
        if (groundedPlayer || (coyote_timer < coyote_time_amount)){
            coyote_timer = coyote_time_amount;
            if (playerVelocity.y < 0f){
                playerVelocity.y = 0f;
            }

            playerVelocity.y = initialJumpVelocity;
        }

        //Double Jump
        else{
            if (double_jump_able && double_jump){
                playerVelocity.y = initialJumpVelocity;
                double_jump = false;

            }   
        }
    }

    private void HandleRotation(Vector3 moveDir){
        if (moveDir.sqrMagnitude < 0.0001f) return;
        
        moveDir.y = 0f;
        moveDir.Normalize();

        Quaternion target = Quaternion.LookRotation(moveDir);
        
        float t = 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
    }
    
    public Vector3 GetInputVector()
    {
        if (!canMove) return Vector3.zero;
        
        inputVector = moveAction.ReadValue<Vector2>();

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 move = (inputVector.x * camRight + inputVector.y * camForward).normalized;

        return move;
    }
    
    public void SetTranslationDisabled(bool disabled)
    {
        translationDisabled = disabled;
    }

    private Vector3 slope_sticking(Vector3 velocity){
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f)){
            var slope_rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjusted_velocity = slope_rotation * velocity;

            if (adjusted_velocity.y > 0){
                return adjusted_velocity;
            }
        }

        return velocity;
    }

    public void SetNewMoveState(moveState newState)
    {
        currentState = newState;
    }

    private void StateChanged()
    {
        switch (currentState)
        {
            case moveState.Idle:
                dash_particle.SetActive(false);
                playerCurrSpeed = playerWalkSpeed;
                break;
            case moveState.Walking:
                dash_particle.SetActive(false);
                PlayParticle(moving_particle);
                break;
            case moveState.Running:
                dash_particle.SetActive(true);
                playerCurrSpeed = playerRunSpeed;
                break;
            case moveState.Dodging:
                playerCurrSpeed = playerRunSpeed;
                SetNewMoveState(moveState.Running);
                break;
        }
    }

    public void PlayParticle(GameObject particle)
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hit_normal = hit.normal;
        /*if (controller.slopeLimit > Vector3.Angle(hit_normal, Vector3.up))
        {
            Debug.Log("WOOOO");
        }

        else
        {
            Debug.Log("AWWWW");
        }*/
    }

    public void KnockbackPlayer(Vector3 dir, float force)
    {
        dir = dir.normalized;
        knockbackTimer = defaultKnockbackTime;
        knockbackForce = new Vector3(dir.x * force, 4f, dir.z * force);
    }
    
    //with custom height
    public void KnockbackPlayer(Vector3 dir, float force, float height)
    {
        dir = dir.normalized;
        knockbackTimer = defaultKnockbackTime;
        knockbackForce = new Vector3(dir.x * force, height, dir.z * force);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Upgrade upgrade))
        {
            if (upgrade.upgrade_type == Upgrade.UpgradeType.TIMESLOW)
            {
                UI_manager.OpenTextScrollMenu("You've absorbed the essence of time! Press E to slow down time.");
                time_able = true;
                // while(UIManager.ui_state == UIManager.UIState.ACTIVE)
                // {
                //     if (Input.GetKeyDown(KeyCode.Q)){
                //         UI_manager.CloseTextScrollMenu();
                //     }
                // }
            }
        }

        if (other.TryGetComponent(out TimeIntangible time_block))
        {
            time_able = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TimeIntangible time_block))
        {
            time_able = true;
        }
    }
}
