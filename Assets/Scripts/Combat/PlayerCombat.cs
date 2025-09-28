using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private GameObject player;
    
    public enum PlayerState
    {
        IDLE, ATTACK, STAGGERED
    }

    public enum CurrentAttack
    {
        Melee1, Melee2, Melee3, Shuriken
    }
    
    private PlayerState state =  PlayerState.IDLE;
    private CurrentAttack currAttack = CurrentAttack.Melee1;
    
    public Animator animator;
    public Collider hitbox;
    
    private int comboNumber;
    private float stateTimer, currBufferTime;
    
    
    [Header("MeleeCombo1")]
    public float inputBuffer = 0.2f;
    public float returnIdleTime = 0.5f;    
    
    [Header("MeleeCombo2")]

    
    [Header("MeleeCombo3")]


    private bool queuedInput = false;
    
    [Header("InputStuff")]
    private InputAction attackAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //animator = player.GetComponent<Animator>();

        attackAction = InputSystem.actions.FindAction("Attack");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attackAction.triggered)
        {
            currBufferTime = inputBuffer;
            queuedInput = true;
        }
        
        if (currBufferTime > 0f)
            currBufferTime -= Time.deltaTime;
        
        switch (state)
        {
            case PlayerState.IDLE:
                if (AttackCheck())
                {
                    
                }
                break;
            
            case PlayerState.ATTACK:
                
                break;
            
            case PlayerState.STAGGERED:
                if (stateTimer <= 0)
                    state = PlayerState.IDLE;
                break;
        }
    }

    bool AttackCheck()
    {
        if (queuedInput && currBufferTime > 0)
        {
            Debug.Log("Attack");
            return true;
        }
        return false;
    }
}
