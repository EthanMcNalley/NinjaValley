using UnityEngine;

public class GroundAttack3 : CombatState
{
    public AttackData.CombatStateID stateID = AttackData.CombatStateID.GroundAttack3;
    public float damage = 3f;
    float stateDuration = 0.65f;
    float bufferDuration = 0.5f;
    public float shadowCharge = 20f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;
        
        Debug.Log("Melee3");
        stateManager.KatanaHitBox.BeginAttack();
        stateManager.playerAnimatior.SetTrigger("Attack1");
        
        AudioManager.instance.PlayOneShot(stateManager.slashSound3, stateManager.transform.position);
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration) || (stateManager.attacking && stateManager.stateTime >= stateDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee1);
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
