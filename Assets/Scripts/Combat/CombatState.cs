using UnityEngine;

public abstract class CombatState
{
    public float duration;
    public float bufferDuration;
    
    public abstract void EnterState(CombatStateManager state);
    
    public abstract void UpdateState(CombatStateManager state);
    
    public abstract void OnCollisionEnter(CombatStateManager state);
    
}
