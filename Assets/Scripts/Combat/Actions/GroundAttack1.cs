using UnityEngine;

public class GroundAttack1 : CombatState
{
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 0.75f;
        bufferDuration = 0.75f;
        Debug.Log("Melee1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee2);
        }
    }

    public override void OnCollisionEnter(CombatStateManager stateManager)
    {
        
    }
}
