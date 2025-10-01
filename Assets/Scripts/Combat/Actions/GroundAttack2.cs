using UnityEngine;

public class GroundAttack2 : CombatState
{
    public override void EnterState(CombatStateManager stateManager)
    {
        duration = 0.5f;
        bufferDuration = 0.5f;
        Debug.Log("Melee2");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (duration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee3);
        }
    }

    public override void OnCollisionEnter(CombatStateManager stateManager)
    {
        
    }
}
