using UnityEngine;
using UnityEngine.InputSystem;

public class CombatStateManager : MonoBehaviour
{
    public CombatState currentState;


    // Current stage of the combo
    private int comboStep = 0;
    
    //State instances
    public Idle Idle = new Idle();
    public GroundAttack1 Melee1 = new GroundAttack1();
    public GroundAttack2 Melee2 = new GroundAttack2();
    public GroundAttack3 Melee3 = new GroundAttack3();
    public Dodge Dodge = new Dodge();
    
    //Hitbox stuff
    [Header ("Hitbox Stuff")]
    public CharacterController characterController;
    public HealthSystem healthSystem;
    public ShadowAssassin shadowAssassin;
    public NewMovement movementController;
    public GameObject GroundAttackHitbox;
    public Animator AttackAnimator;
    public Collider GroundHitboxCollider;
    public float currentDamage;
    public float shadowCharge;
    
    //Input Stuff
    [Header ("Input Stuff")]
    private InputAction attackAction;
    private InputAction dodgeAction;
    private InputAction kunaiAction;
    public bool attacking = false;
    public float stateTime = 0f;
    private float bufferTime = 0f;
    public float bufferDurationTimer = 1f;
    
    [Header ("Dodge Stuff")]
    public float DodgeCoolDown = 1f;
    public float DodgeCoolDownTimer = 0f;
    private float lastDodgeTime = 0f;
    
    [Header ("Kunai Stuff")]
    public GameObject Kunai;
    public GameObject kunaiPosition;
    
    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        dodgeAction = InputSystem.actions.FindAction("Dodge");
        kunaiAction = InputSystem.actions.FindAction("Kunai");

        characterController = GetComponent<CharacterController>();
        movementController = GetComponent<NewMovement>();
        healthSystem =  GetComponent<HealthSystem>();
        shadowAssassin = GetComponent<ShadowAssassin>();

        GroundHitboxCollider = GroundAttackHitbox.gameObject.GetComponent<Collider>();

        
        AttackAnimator = GetComponent<Animator>();
        
        currentState = Idle;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        stateTime += Time.deltaTime;
        if (DodgeCoolDownTimer <= DodgeCoolDown)
        {
            DodgeCoolDownTimer += Time.deltaTime;
        }
        AttackCheck();
        DodgeCheck();
        KunaiCheck();
        currentState.UpdateState(this);
    }

    private void AttackCheck()
    {
        if (attackAction.triggered)
        {
            attacking = true;
        }

        if (bufferTime > bufferDurationTimer)
        {
            bufferTime = 0f;
            attacking = false;
        }
        
        if (attacking)
        {
            bufferTime += Time.deltaTime;
        }
        else
        {
            bufferTime = 0f;
        }
    }

    private void KunaiCheck()
    {
        //Commented out for now cuz animation is broken
        // if (kunaiAction.triggered)
        // {
        //     AttackAnimator.SetTrigger("Kunai");
        //     Instantiate(Kunai, kunaiPosition.transform.position, gameObject.transform.rotation);
        // }
    }

    private void DodgeCheck()
    {
        if (dodgeAction.triggered && DodgeCoolDownTimer >= DodgeCoolDown)
        {
            SwitchState(Dodge);
            DodgeCoolDownTimer = 0f;
        }
    }

    public void SwitchState(CombatState state)
    {
        currentState.ExitState(this);
        currentState = state;
        //Reset for new state
        stateTime = 0f;
        state.EnterState(this);
    }

    public void StartCombo()
    {
        comboStep = 1;
        attacking = false;
        SwitchState(Melee1);
    }

    public float GetDamage()
    {
        return currentDamage;
    }

    public float GetShadowCharge()
    {
        return shadowCharge;
    }

    public void ContinueCombo(CombatState nextState)
    {
        if (attacking && nextState != null)
        {
            attacking = false;
            comboStep++;
            SwitchState(nextState);
        }
        else
        {
            comboStep = 0;
            SwitchState(Idle);
        }
    }
}
