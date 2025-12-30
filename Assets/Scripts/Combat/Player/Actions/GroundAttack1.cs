using UnityEngine;

public class GroundAttack1 : CombatState
{
    public AttackData.CombatStateID stateID = AttackData.CombatStateID.GroundAttack1;
    public float damage = 1f;
    float stateDuration = 0.4f;
    float bufferDuration = 0.7f;
    public float shadowCharge = 20f;

    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;

        Debug.Log("Melee1");
        stateManager.KatanaHitBox.BeginAttack();
        stateManager.playerAnimatior.SetTrigger("Attack1");
        
        AudioManager.instance.PlayOneShot(stateManager.slashSound, stateManager.transform.position);
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration) || (stateManager.attacking && stateManager.stateTime >= stateDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee2);
        }
        
        if (stateManager.stateTime >= stateDuration && stateManager.KatanaHitBox.katanaHitbox.enabled)
        {
            stateManager.KatanaHitBox.EndAttack();
        }
    }

    public override void ExitState(CombatStateManager stateManager)
    {
        stateManager.movementController.EnableMovement();
        stateManager.KatanaHitBox.EndAttack();
    }
}
