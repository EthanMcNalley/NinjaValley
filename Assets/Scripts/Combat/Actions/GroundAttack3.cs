using UnityEngine;

public class GroundAttack3 : CombatState
{
    public override void EnterState(CombatStateManager stateManager)
    {
        duration = 1f;
        bufferDuration = 0.5f;
        Debug.Log("Melee3");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= duration + bufferDuration)
        {
            stateManager.ContinueCombo(stateManager.Melee1);
        }
    }

    public override void OnCollisionEnter(CombatStateManager stateManager)
    {
        
    }
}
