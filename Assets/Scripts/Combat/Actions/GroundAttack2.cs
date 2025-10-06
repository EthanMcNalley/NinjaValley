using UnityEngine;

public class GroundAttack2 : CombatState
{
    public float damage = 2f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 0.5f;
        bufferDuration = 0.5f;
        stateManager.currentDamage = damage;
        Debug.Log("Melee2");
        stateManager.AttackAnimation.SetTrigger("Attack1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee3);
        }
    }

    /*public override void OnCollisionEnter(Collider other)
    {
        
    }*/
}
