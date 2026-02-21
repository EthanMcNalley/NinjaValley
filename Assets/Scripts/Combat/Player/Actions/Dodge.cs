using UnityEngine;

public class Dodge : CombatState
{
    float stateDuration = 0.3f;
    private float dodgeTimer = 0f;
    private float iframeEndTime = 0.29f;
    private bool isInvincible = false;
    private float dodgeSpeed = 80f;
    private float gracePeriod = 0.05f;
    private bool movementDisabled = false;
    private Vector3 dir;
    //private float gravity = -30f;
    
    public override void EnterState(CombatStateManager stateManager)
    {
        Debug.Log("Dodge");
        dir = GetDodgeDir(stateManager);
        dodgeTimer = 0f;
        movementDisabled = false;
        isInvincible = true;
        //gravity = stateManager.movementController.gravityValue;
        
        if (stateManager.healthSystem != null) stateManager.healthSystem.SetInvincible(true);
        stateManager.movementController.DodgeStart();
        //stateManager.movementController.SetTranslationDisabled(true);
        //stateManager.movementController.SetNewMoveState(NewMovement.moveState.Dodging);
        
        if (dir == Vector3.zero)
        {
            dir = stateManager.transform.forward;
        }
        
        stateManager.playerAnimatior.SetTrigger("Dash");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= stateDuration)
        {
            stateManager.SwitchState(stateManager.Idle);
        }

        if (dodgeTimer < gracePeriod)
        {
            // During the grace period, let the player still adjust facing direction
            dir = GetDodgeDir(stateManager);
            if (dir == Vector3.zero)
                dir = stateManager.transform.forward;
        }
        else if (!movementDisabled)
        {
            // Once the grace period ends, disable further rotation/input
            stateManager.movementController.DisableMovement();
            movementDisabled = true;
            var rot = Quaternion.LookRotation(dir);
            stateManager.transform.rotation = rot;
        }

        if (dodgeTimer >= iframeEndTime && isInvincible)
        {
            if (stateManager.healthSystem != null) stateManager.healthSystem.SetInvincible(false);
            isInvincible = false;
        }
        
        float t = Mathf.Clamp01(dodgeTimer / stateDuration);
        float easeOut = 1f - t * t;
        
        stateManager.characterController.Move(dodgeSpeed * easeOut * Time.deltaTime * dir);
        
        //just to keep the player grounded
        stateManager.characterController.Move(-0.5f * Time.deltaTime * Vector3.up);


        dodgeTimer += Time.deltaTime;
    }
    
    public override void ExitState(CombatStateManager stateManager)
    {
        if (stateManager.healthSystem != null) stateManager.healthSystem.SetInvincible(false);
        stateManager.movementController.DodgeEnd();
    }

    private Vector3 GetDodgeDir(CombatStateManager stateManager)
    {
        return stateManager.movementController.GetInputVector();
    }
}
