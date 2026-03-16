using UnityEngine;

public class BossIdle : BossState
{
    public BossAttackType BossAttackType =  BossAttackType.Idle;
    public float stateTime = 3f;
    private float currentStateTime = 3f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = stateTime;
        //Animation here
        Debug.Log("Boss Idle");
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0)
        {
            boss.ChoseAttack();
        }
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}

public class BossPhase1NormalAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1NormalAttack;
    public float stateTime = 4f;
    private float currentStateTime = 4f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = boss.bossHpSystem.maxBreakTimer;
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0)
        {
            boss.ChoseAttack();
        }
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}

public class BossPhase1TileAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1TileAttack;
    public float stateTime = 8f;
    private float currentStateTime = 8f;
    public bool attackAnimationTriggered;
    private PoopAttack currentPoopAttack;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        Debug.Log("BossPhase1TileAttack");
        currentStateTime = stateTime;
        attackAnimationTriggered =  false;
        currentPoopAttack = boss.poopAttackPatternSelector();
        currentPoopAttack.pointing.SetActive(true);
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;

        if (currentStateTime <= 0)
        {
            boss.ChoseAttack();
        }
    }
    
    public override void ExitState(BossManager bossManager)
    {
        Debug.Log("Exit tile attack");
        currentPoopAttack.pointing.SetActive(false);
        boss.tileAttackAnimator.SetTrigger("Poop" + currentPoopAttack.thePoop);
    }

}

public class BossPhase1DoorWordAttack : BossState
{
    public BossAttackType BossStateType = BossAttackType.Phase1DoorWordAttack;
    public float stateTime = 30f;
    private float currentStateTime = 30f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = stateTime;
        boss.portalTopIndicator.SetActive(true);
        boss.portalIndicatorUI.ClearPortals();
        boss.NeededPortalColors();
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0 || boss.portalIndicatorUI.cleared)
        {
            boss.ChoseAttack();
        }
    }
    
    public override void ExitState(BossManager bossManager)
    {
        if (!boss.portalIndicatorUI.cleared)
        {
            boss.playerHealth.TakeDamage(20f);
        }
        boss.portalTopIndicator.SetActive(false);
        boss.portalIndicatorUI.ClearPortals();
    }
}

public class BossBreak : BossState
{
    public BossAttackType BossStateType = BossAttackType.Break;
    public float stateTime = 6.7f;
    private float currentStateTime = 6.7f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = stateTime;
        //Animation here

    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0)
        {
            boss.SwitchState(boss.bossIdle);
        }
    }
    
    public override void ExitState(BossManager bossManager)
    {
        
    }
}


public class Temp : BossState
{
    //public BossAttackType BossStateType = BossAttackType.Phase1DoorWordAttack;
    public float stateTime = 6.7f;
    private float currentStateTime = 6.7f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = stateTime;
        //Animation here
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
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
