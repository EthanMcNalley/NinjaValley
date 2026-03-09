using UnityEngine;

public class BossIdle : BossState
{
    public BossAttackType BossAttackType =  BossAttackType.Idle;

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
    
    public override void ExitState()
    {
        
    }
}

public class BossPhase1NormalAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1NormalAttack;

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
    
    public override void ExitState()
    {
        
    }
}

public class BossPhase1TileAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1TileAttack;

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
    
    public override void ExitState()
    {
        
    }
}

public class BossPhase1DoorWordAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1DoorWordAttack;

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
    
    public override void ExitState()
    {
        
    }
}


public enum BossAttackType
{
    Idle,
    Phase1NormalAttack,
    Phase1TileAttack,
    Phase1DoorWordAttack
}
