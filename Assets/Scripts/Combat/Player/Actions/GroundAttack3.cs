using UnityEngine;

public class GroundAttack3 : CombatState
{
    public float damage = 3f;
    float stateDuration = 0.5f;
    float bufferDuration = 0.5f;
    public float shadowCharge = 20f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;
        
        Debug.Log("Melee3");
        stateManager.GroundHitboxCollider.enabled = true;
        stateManager.AttackAnimator.SetTrigger("Attack1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration) || (stateManager.attacking && stateManager.stateTime >= stateDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee1);
        }

        if (stateManager.stateTime >= stateDuration && stateManager.GroundHitboxCollider.enabled)
        {
            stateManager.GroundHitboxCollider.enabled = false;
        }
    }

    public override void ExitState(CombatStateManager stateManager)
    {
        stateManager.movementController.EnableMovement();
        stateManager.GroundHitboxCollider.enabled = false;
    }
}
