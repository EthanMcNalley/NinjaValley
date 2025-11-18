using UnityEngine;

public class GroundAttack2 : CombatState
{
    public float damage = 2f;
    float stateDuration = 0.5f;
    float bufferDuration = 0.5f;
    public float shadowCharge = 20f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;
        Debug.Log("Melee2");
        stateManager.KatanaEnableAnimator.SetTrigger("Attack1");
        stateManager.AttackAnimator.SetTrigger("Attack2");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee3);
        }
    }

    public override void ExitState(CombatStateManager stateManager)
    {
        return;
    }
}
