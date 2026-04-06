using UnityEngine;

public class BossPhase1Idle : BossState
{
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
    public float stateTime = 4f;
    private float currentStateTime = 4f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = boss.bossHpSystem.maxBreakTimer;
        boss.normalAttacked++;
        boss.InstantiateNormalAttack();
        //Animation here
        bossManager.animator.SetTrigger("Basic");
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
        bossManager.animator.ResetTrigger("Basic");
    }
}

public class BossPhase1TileAttack : BossState
{
    public float stateTime = 10f;
    private float currentStateTime = 10f;
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
        bossManager.animator.SetTrigger("Poop");
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
        bossManager.animator.ResetTrigger("Poop");
    }

}

public class BossPhase1DoorWordAttack : BossState
{
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
        bossManager.animator.SetBool("Portal", true);
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
            boss.playerHealth.TakeDamage(30f);
        }
        boss.portalTopIndicator.SetActive(false);
        boss.portalIndicatorUI.ClearPortals();
        bossManager.animator.SetBool("Portal", false);
    }
}

public class BossPhase1Break : BossState
{
    public float stateTime = 6.7f;
    private float currentStateTime = 6.7f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = stateTime;
        boss.normalAttacked = 0;
        //Animation here
        bossManager.animator.SetBool("Breaking", true);
        bossManager.animator.SetTrigger("Break");
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
        bossManager.animator.SetBool("Breaking", false);
    }
}

public class BossPhase1Transition : BossState
{
    //public BossAttackType BossStateType = BossAttackType.Phase1DoorWordAttack;
    public float stateTime = 60f;
    private float currentStateTime = 60f;
    private FinalBossPhase1 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss =  bossManager as FinalBossPhase1;
        currentStateTime = stateTime;
        //PLAY BREAK ANIMATION THEN WAVEY
        bossManager.animator.SetBool("Break", true);
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
    }
    
    public override void ExitState(BossManager bossManager)
    {
        bossManager.animator.SetBool("Break", false);
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


