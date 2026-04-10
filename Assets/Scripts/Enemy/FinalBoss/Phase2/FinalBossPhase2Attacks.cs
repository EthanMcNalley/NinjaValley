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
        
        if (boss.distToPlayer >= 11f)
        {
            Vector3 targetPos = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
            boss.transform.position =
                Vector3.MoveTowards(boss.transform.position, targetPos, boss.moveSpeed * Time.deltaTime);
        }

        boss.animator.SetBool("isWalking", boss.distToPlayer >= 11f);

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
    public float stateTime = 3f;
    private float currentStateTime;
    private FinalBossPhase2 boss;

    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        boss.animator.SetTrigger("Attack3");
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

public class BossPhase2AirAttack : BossState
{
    public float stateTime = 8f;
    public float travelTime = 1.5f; // how long the dive takes regardless of distance
    private float currentStateTime;
    private Vector3 lockedTargetPos;
    private float speed;
    private FinalBossPhase2 boss;
 
    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;

        boss.rotationSpeed = 6f;
        lockedTargetPos = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);

        float dist = Vector3.Distance(boss.transform.position, lockedTargetPos);
        speed = dist / travelTime;
 
        boss.animator.SetTrigger("Attack2");
        Debug.Log("Phase2 Air Attack");
    }
 
    public override void UpdateState(BossManager bossManager)
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, lockedTargetPos, speed * Time.deltaTime);
 
        currentStateTime -= Time.deltaTime;
        if (currentStateTime <= 0)
        {
            boss.ChoseAttack();
        }
    }
 
    public override void ExitState(BossManager bossManager)
    {
        boss.rotationSpeed = 1f;
        boss.DisableAirAttackHitBox();
    }
}

public class BossPhase2RealmAttack : BossState
{
    public float stateTime = 60f;
    public float penaltyDamage = 40f;
    private float currentStateTime;
    private bool realmCompleted;
    private FinalBossPhase2 boss;
 
    public override void EnterState(BossManager bossManager)
    {
        boss = bossManager as FinalBossPhase2;
        currentStateTime = stateTime;
        realmCompleted = false;
        boss.realmAttackActive = true;
        boss.activeGate1.GetComponent<BoxCollider>().enabled = true;
        boss.activeGate2.GetComponent<BoxCollider>().enabled = true;
        boss.activeGate1.GetComponent<Animator>().SetBool("Open", true);
        boss.activeGate2.GetComponent<Animator>().SetBool("Open", true);
 
        boss.activeRealmPortal.SetActive(true);
        
        boss.moveSpeed = 0f;
        boss.rotationSpeed = 0f;

        boss.bossHpSystem.damageMod = 0f;
 
        FinalBossPhase2.OnRealmEnemyKilled += OnEnemyKilled;
        
        Debug.Log("portal open");
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
        FinalBossPhase2.OnRealmEnemyKilled -= OnEnemyKilled;
        boss.activeRealmPortal.SetActive(false);
        boss.activeGate1.GetComponent<BoxCollider>().enabled = false;
        boss.activeGate2.GetComponent<BoxCollider>().enabled = false;
        boss.activeGate1.GetComponent<Animator>().SetBool("Open", false);
        boss.activeGate2.GetComponent<Animator>().SetBool("Open", false);
 
        if (!realmCompleted)
        {
            boss.playerHealth.TakeDamage(penaltyDamage);
            boss.player.transform.position = boss.realmReturnPosition.position;
            boss.realmAttackActive = false;
        }
 
        // Restore movement
        boss.moveSpeed = 5f;
        boss.rotationSpeed = 10f;
        boss.bossHpSystem.damageMod = boss.bossHpSystem.defaultdamageMod;
    }
 
    private void OnEnemyKilled()
    {
        realmCompleted = true;
        boss.OnRealmComplete();
    }
}

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
    }

    public override void UpdateState(BossManager bossManager)
    {
        currentStateTime -= Time.deltaTime;
    }

    public override void ExitState(BossManager bossManager) { }
}