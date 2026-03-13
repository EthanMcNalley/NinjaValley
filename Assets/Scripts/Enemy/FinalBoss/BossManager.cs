using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public bool inCombat;
    public bool isDead;
    
    public BossState currentState;
    public float currentStateTime;

    protected virtual void Start()
    {
        currentState?.EnterState(this);
    }
    
    protected virtual void Update()
    {
        if (currentState == null) return;

        if (inCombat)
        {
            currentStateTime += Time.deltaTime;
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(BossState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        //Reset for new state
        currentStateTime = 0f;
        
        Debug.Log("Switching to current state -> " + currentState);
        
        state.EnterState(this);
    }
}
