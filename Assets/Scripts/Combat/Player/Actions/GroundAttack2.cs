using UnityEngine;

public class GroundAttack2 : CombatState
{
    public AttackData.CombatStateID stateID = AttackData.CombatStateID.GroundAttack2;
    public float damage = 2f;
    float stateDuration = 0.45f;
    float bufferDuration = 0.7f;
    public float shadowCharge = 20f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;
        
        Debug.Log("Melee2");
        stateManager.KatanaHitBox.BeginAttack();
        stateManager.playerAnimatior.SetTrigger("Attack2");
        stateManager.slashVFX[1].slashVFX.SetActive(true);
        
        AudioManager.instance.PlayOneShot(stateManager.slashSound2, stateManager.transform.position);
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration) || (stateManager.attacking && stateManager.stateTime >= stateDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee3);
        }
        
        if (stateManager.stateTime >= stateDuration && stateManager.KatanaHitBox.katanaHitbox.enabled)
        {
            stateManager.KatanaHitBox.EndAttack();
        }
    }

    public override void ExitState(CombatStateManager stateManager)
    {
        stateManager.slashVFX[1].slashVFX.SetActive(false);
        stateManager.movementController.EnableMovement();
        stateManager.KatanaHitBox.EndAttack();
    }
}
