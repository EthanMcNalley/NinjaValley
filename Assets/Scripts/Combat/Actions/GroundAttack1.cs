using UnityEngine;

public class GroundAttack1 : CombatState
{
    public override void EnterState(CombatStateManager stateManager)
    {
        duration = 0.5f;
        bufferDuration = 0.5f;
        Debug.Log("Melee1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (duration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee2);
        }
    }

    public override void OnCollisionEnter(CombatStateManager stateManager)
    {
        
    }
}
