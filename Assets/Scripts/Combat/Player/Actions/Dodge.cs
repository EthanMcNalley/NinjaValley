using UnityEngine;

public class Dodge : CombatState
{
    float stateDuration = 0.3f;
    private float dodgeTimer = 0f;
    private float iframeEndTime = 0.15f;
    private bool isInvincible = false;
    private float dodgeSpeed = 50f;
    private float gracePeriod = 0.05f;
    private bool movementDisabled = false;
    private Vector3 dir;
    
    public override void EnterState(CombatStateManager stateManager)
    {
        Debug.Log("Dodge");
        dir = GetDodgeDir(stateManager);
        dodgeTimer = 0f;
        movementDisabled = false;
        isInvincible = true;
        if (stateManager.healthSystem != null) stateManager.healthSystem.SetInvincible(true);
        stateManager.movementController.SetTranslationDisabled(true);
        
        if (dir == Vector3.zero)
        {
            dir = stateManager.transform.forward;
        }
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
        }

        if (dodgeTimer >= iframeEndTime && isInvincible)
        {
            if (stateManager.healthSystem != null) stateManager.healthSystem.SetInvincible(false);
            isInvincible = false;
        }
        
        float t = Mathf.Clamp01(dodgeTimer / stateDuration);
        float easeOut = 1f - t * t;
        
        stateManager.characterController.Move(dir * dodgeSpeed * easeOut * Time.deltaTime);
        
        dodgeTimer += Time.deltaTime;
    }
    
    public override void ExitState(CombatStateManager stateManager)
    {
        stateManager.movementController.SetTranslationDisabled(false);
        stateManager.movementController.EnableMovement();
    }

    private Vector3 GetDodgeDir(CombatStateManager stateManager)
    {
        return stateManager.movementController.GetInputVector();
    }
}
