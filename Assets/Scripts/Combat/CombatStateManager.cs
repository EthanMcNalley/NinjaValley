using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatStateManager : MonoBehaviour
{
    public CombatState currentState;

    // Current stage of the combo
    public int comboStep = 0;
    
    //State instances
    public Idle Idle = new Idle();
    public GroundAttack1 Melee1 = new GroundAttack1();
    public GroundAttack2 Melee2 = new GroundAttack2();
    public GroundAttack3 Melee3 = new GroundAttack3();
    
    
    //Input Stuff
    private InputAction attackAction;
    public bool attacking = false;
    public float stateTime = 0f;
    
    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        
        currentState =  Idle;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        stateTime += Time.deltaTime;
        if (attackAction.triggered)
        {
            attacking = true;
        }
        
        currentState.UpdateState(this);
    }

    public void SwitchState(CombatState state)
    {
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
