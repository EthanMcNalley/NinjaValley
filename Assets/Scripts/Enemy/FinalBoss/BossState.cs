using UnityEngine;

public abstract class BossState
{
    public abstract void EnterState(BossManager bossManager);
    
    public abstract void UpdateState(BossManager bossManager);
    
    public abstract void ExitState(BossManager bossManager);
}