using UnityEngine;

public class Dodge : CombatState
{
    float stateDuration = 1f;
    public override void EnterState(CombatStateManager stateManager)
    {
        Debug.Log("Dodge");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= stateDuration)
        {
            stateManager.SwitchState(stateManager.Idle);
        }
    }
}
