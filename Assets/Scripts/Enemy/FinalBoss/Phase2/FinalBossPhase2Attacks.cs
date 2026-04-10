using UnityEngine;

public class BossPhase2Idle : BossState
{
    public float stateTime = 5f;
    private float currentStateTime;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        // Animation: walk
        Debug.Log("Phase2 Boss Idle");
    }

    public override void UpdateState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        // Move toward player
        Vector3 targetPos = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPos, boss.moveSpeed * Time.deltaTime);
        
        boss.animator.SetBool("isWalking", boss.distToPlayer >= 5f);

        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0)
        {
            boss.ChoseAttack();
        }
    }

    public override void ExitState(BossManager bossManager)
    {
        // Animation: stop walk
        boss.animator.SetBool("isWalking", false);
    }
}

public class BossPhase2BeamAttack : BossState
{
    public float stateTime = 8f;
    private float currentStateTime;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        boss.animator.SetTrigger("Attack1");
        Debug.Log("Phase2 Beam Attack");
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
        boss.DeactivateBeamVFX();
        boss.DisableBeamHitBox();
    }
}

public class BossPhase2HowlingAttack : BossState
{
    public float stateTime = 4f;
    private float currentStateTime;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        // Animation: trigger Attack1 (howling)
        Debug.Log("Phase2 Howling Attack");
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
        boss.DisableHowlingHitBox();
    }
}

// -------------------------------------------------------
// AIR ATTACK — flies toward player then slams
// -------------------------------------------------------
public class BossPhase2AirAttack : BossState
{
    public float stateTime = 8f;
    private float currentStateTime;
    private bool isFlying;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        isFlying = true;
        boss.animator.SetTrigger("Attack2");
        Debug.Log("Phase2 Air Attack");
    }

    public override void UpdateState(BossManager bossManager)
    {
        if (isFlying)
        {
            // Follow player in air until close
            Vector3 targetPos = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPos, 15f * Time.deltaTime);

            if (boss.distToPlayer <= 2f)
            {
                isFlying = false;
                // Animation: trigger slam
            }
        }

        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0)
        {
            boss.ChoseAttack();
        }
    }

    public override void ExitState(BossManager bossManager)
    {
        boss.DisableAirAttackHitBox();
    }
}

// -------------------------------------------------------
// BREAK
// -------------------------------------------------------
public class BossPhase2Break : BossState
{
    public float stateTime = 6.7f;
    private float currentStateTime;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        boss.normalAttacked = 0;
        // Animation: break
        bossManager.animator.SetBool("IsBreak", true);
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
        bossManager.animator.SetBool("IsBreak", false);
    }
}

public class BossPhase2Transition : BossState
{
    public float stateTime = 60f;
    private float currentStateTime;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        // Animation: death/wave cutscene
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
    }

    public override void ExitState(BossManager bossManager) { }
}