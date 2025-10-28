using UnityEngine;

public class GroundAttack1 : CombatState
{
    public float damage = 1f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 0.5f;
        bufferDuration = 0.5f;
        stateManager.currentDamage = damage;
        Debug.Log("Melee1");
        stateManager.KatanaEnableAnimator.SetTrigger("Attack1");
        stateManager.AttackAnimator.SetTrigger("Attack1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee2);
        }
    }

    /*public override void OnCollisionEnter(Collider other)
    {

    }*/
    
}
