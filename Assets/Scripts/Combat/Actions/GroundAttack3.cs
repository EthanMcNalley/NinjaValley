using UnityEngine;

public class GroundAttack3 : CombatState
{
    public float damage = 3f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateDuration = 1f;
        bufferDuration = 0.75f;
        stateManager.currentDamage = damage;
        Debug.Log("Melee3");
        stateManager.AttackAnimation.SetTrigger("Attack1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= stateDuration + bufferDuration)
        {
            stateManager.ContinueCombo(stateManager.Melee1);
        }
    }

    /*public override void OnCollisionEnter(Collider other)
    {
        
    }*/
}
