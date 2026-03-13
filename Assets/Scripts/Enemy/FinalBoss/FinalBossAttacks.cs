using UnityEngine;

public class BossIdle : BossState
{
    public BossAttackType BossAttackType =  BossAttackType.Idle;
    public float statetime;

    public override void EnterState(BossManager bossManager)
    {
        //Animation here
        Debug.Log("Boss Idle");
    }

    public override void UpdateState(BossManager bossManager)
    {
        
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}

public class BossPhase1NormalAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1NormalAttack;
    public float statetime;

    public override void EnterState(BossManager bossManager)
    {
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}

public class BossPhase1TileAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1TileAttack;
    public float statetime;

    public override void EnterState(BossManager bossManager)
    {
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}

public class BossPhase1DoorWordAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1DoorWordAttack;
    public float statetime;

    public override void EnterState(BossManager bossManager)
    {
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}

public class BossBreak : BossState
{
    public BossAttackType BossStateType = BossAttackType.Break;
    public float statetime;

    public override void EnterState(BossManager bossManager)
    {
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}


public enum BossAttackType
{
    Idle,
    Phase1NormalAttack,
    Phase1TileAttack,
    Phase1DoorWordAttack,
    Break
}
