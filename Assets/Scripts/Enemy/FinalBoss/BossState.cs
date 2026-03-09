using UnityEngine;

public abstract class BossState
{
    public abstract void EnterState();
    
    public abstract void UpdateState();
    
    public abstract void ExitState();
}