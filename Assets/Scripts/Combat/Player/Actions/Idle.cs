using UnityEngine;

public class Idle : CombatState
{
    public AttackData.CombatStateID stateID = AttackData.CombatStateID.Idle;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        Debug.Log("Idle");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.attacking)
        {
            stateManager.StartCombo();
        }
    }
    
    public override void ExitState(CombatStateManager stateManager)
    {
        return;
    }

    /*public override void OnCollisionEnter(Collider other)
    {
        
    }*/
}
