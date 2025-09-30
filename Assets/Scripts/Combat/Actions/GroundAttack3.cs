using UnityEngine;

public class GroundAttack3 : CombatState
{
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 1f;
        bufferDuration = 0.75f;
        Debug.Log("Melee3");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= stateDuration + bufferDuration)
        {
            stateManager.ContinueCombo(stateManager.Melee1);
        }
    }

    public override void OnCollisionEnter(CombatStateManager stateManager)
    {
        
    }
}
