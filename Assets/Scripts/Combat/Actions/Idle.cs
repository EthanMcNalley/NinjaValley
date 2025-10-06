using UnityEngine;

public class Idle : CombatState
{
    
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 0f;
        bufferDuration = 0f;
        Debug.Log("Idle");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.attacking)
        {
            stateManager.StartCombo();
        }
    }

    /*public override void OnCollisionEnter(Collider other)
    {
        
    }*/
}
