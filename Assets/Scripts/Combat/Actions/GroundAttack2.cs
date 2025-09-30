using UnityEngine;

public class GroundAttack2 : CombatState
{
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 0.75f;
        bufferDuration = 0.75f;
        Debug.Log("Melee2");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee3);
        }
    }

    public override void OnCollisionEnter(CombatStateManager stateManager)
    {
        
    }
}
