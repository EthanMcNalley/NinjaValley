using UnityEngine;

public class GroundAttack1 : CombatState
{
    private AttackData.CombatStateID stateID = AttackData.CombatStateID.GroundAttack1;
    private float damage = 1f;
    float stateDuration = 0.45f;
    float bufferDuration = 0.7f;
    //public float shadowCharge = 20f;

    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        stateManager.currentDamage = damage;
        //stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;
        
        //Debug.Log("Melee1");
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
        if (stateManager.slashVFX != null && stateManager.slashVFX.Count > 0 && stateManager.slashVFX[0] != null)
        {
            stateManager.slashVFX[0].slashVFX.SetActive(false);
        }

        stateManager.movementController.EnableMovement();
        stateManager.KatanaHitBox.EndAttack();
    }
}

public class GroundAttack2 : CombatState
{
    private AttackData.CombatStateID stateID = AttackData.CombatStateID.GroundAttack2;
    private float damage = 2f;
    float stateDuration = 0.45f;
    float bufferDuration = 0.7f;
    //public float shadowCharge = 20f;
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        stateManager.currentDamage = damage;
        //stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;
        
        //Debug.Log("Melee2");
        stateManager.KatanaHitBox.BeginAttack();
        stateManager.playerAnimatior.SetTrigger("Attack2");
        
        
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
        if (stateManager.slashVFX != null && stateManager.slashVFX.Count > 0 && stateManager.slashVFX[1] != null)
        {
            stateManager.slashVFX[1].slashVFX.SetActive(false);
        }
        stateManager.movementController.EnableMovement();
        stateManager.KatanaHitBox.EndAttack();
    }
}

public class GroundAttack3 : CombatState
{
    private AttackData.CombatStateID stateID = AttackData.CombatStateID.GroundAttack3;
    private float damage = 3f;
    float stateDuration = 0.65f;
    float bufferDuration = 0.5f;
    private float shadowCharge = 20f;
    
    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentStateID = stateID;
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;
        stateManager.bufferDurationTimer =  bufferDuration;
        
        //Debug.Log("Melee3");
        stateManager.KatanaHitBox.BeginAttack();
        stateManager.playerAnimatior.SetTrigger("Attack3");
        
        
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
        if (stateManager.slashVFX != null && stateManager.slashVFX.Count > 0 && stateManager.slashVFX[2] != null)
        {
            stateManager.slashVFX[2].slashVFX.SetActive(false);
        }
        
        //stateManager.movementController.EnableMovement();
        stateManager.KatanaHitBox.EndAttack();
    }
}

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


