using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
    public Animator playerAnimatior;
    public AttackHitBox KatanaHitBox;
    public AttackData.CombatStateID currentStateID;
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
    
    [Header("VFX Stuff")]
    public List<SlashVFX>  slashVFX;
    
    [Header ("Dodge Stuff")]
    public float DodgeCoolDown = 1f;
    public float DodgeCoolDownTimer = 0f;
    [SerializeField]private bool perfectDodgeWindow = false;
    public GameObject clone;
    private bool cloneSpawnedThisDodge = false;
    
    [Header ("Dodge Dash")]
    [SerializeField]private bool dashToEnemy  = false;
    public float dashRadius;
    public LayerMask enemyLayer;
    public float dashSpeed;
    
    
    [Header ("Kunai Stuff")]
    public GameObject Kunai;
    public GameObject kunaiPosition;
    private float kunaiTimer;
    public float kunaiChargeCooldown;
    public float maxKunai = 3;
    public float currentKunai;

    [Header("Sound Stuff")] 
    public EventReference slashSound;
    public EventReference slashSound2;
    public EventReference slashSound3;
    
    public event Action<AttackData> PlayerAttack;

    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        dodgeAction = InputSystem.actions.FindAction("Dodge");
        kunaiAction = InputSystem.actions.FindAction("Kunai");

        characterController = GetComponent<CharacterController>();
        movementController = GetComponent<NewMovement>();
        healthSystem =  GetComponent<HealthSystem>();
        shadowAssassin = GetComponent<ShadowAssassin>();
        
        KatanaHitBox = GroundAttackHitbox.gameObject.GetComponent<AttackHitBox>();
        
        playerAnimatior = GetComponent<Animator>();
        //DisableSlashVFX();
        
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
        KunaiRefill();
        
        DashToEnemy();
        
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
        if (kunaiAction.triggered && currentKunai > 0)
        {
            Instantiate(Kunai, kunaiPosition.transform.position, kunaiPosition.transform.rotation);
            currentKunai--;
        }
    }
    
    private void KunaiRefill()
    {
        if (currentKunai < maxKunai)
        {
            kunaiTimer += Time.deltaTime;

            if (kunaiTimer >= kunaiChargeCooldown)
            {
                currentKunai++;
                kunaiTimer = 0;
            }
        }
    }

    private void DodgeCheck()
    {
        if (stateTime >= 0.3)
        {
            perfectDodgeWindow = false;
        }
        
        if (dodgeAction.triggered && DodgeCoolDownTimer >= DodgeCoolDown)
        {
            SwitchState(Dodge);
            DodgeCoolDownTimer = 0f;
        }

        if (perfectDodgeWindow && !cloneSpawnedThisDodge)
        {
            Instantiate(clone, transform.position, transform.rotation);
            cloneSpawnedThisDodge = true;
            dashToEnemy = true;
        }
    }

    private void DashToEnemy()
    {
        if (dashToEnemy)
        {
            GameObject closestEnemy = FindClosest.FindClosestGameObject(transform.position, dashRadius, enemyLayer);
            
            if (closestEnemy != null)
            {
                characterController.Move(dashSpeed * Time.deltaTime *
                                         (closestEnemy.transform.position - transform.position).normalized);
            }
        }
    }

    public void SwitchState(CombatState state)
    {
        currentState.ExitState(this);
        currentState = state;
        //Reset for new state
        stateTime = 0f;
        
        if (state == Dodge)
        {
            cloneSpawnedThisDodge = false;
        }

        /*if (dashToEnemy)
        {
            DashToEnemy();
        }*/
        
        state.EnterState(this);
        RaisePlayerAttack();
    }

    public void StartCombo()
    {
        comboStep = 1;
        attacking = false;
        SwitchState(Melee1);
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
    
    public void RaisePlayerAttack()
    {
        if (currentState == Idle || currentState == Dodge) return;

        AttackData data = new AttackData(
            currentDamage,
            transform.position,
            transform.rotation,
            currentStateID
        );
        
        PlayerAttack?.Invoke(data);
    }
    
    public float GetDamage()
    {
        return currentDamage;
    }

    public float GetShadowCharge()
    {
        return shadowCharge;
    }

    public void SetPerfectDodgeWindow(bool isPerfect)
    {
        perfectDodgeWindow = isPerfect;
    }

    [Serializable]
    public class SlashVFX
    {
        public GameObject slashVFX;
        public float delay;
    }

    void DisableSlashVFX()
    {
        foreach (var slash in slashVFX)
        {
            slash.slashVFX.SetActive(false);
        }
    }
}
