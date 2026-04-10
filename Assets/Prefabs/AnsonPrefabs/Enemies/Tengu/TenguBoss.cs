using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using FMODUnity;

public class TenguBoss : MonoBehaviour
{
    public Animator animator;
    [SerializeField] GameObject TornadoVFX, RockSpikeVFX, groundStompVFX;
    [SerializeField] GameObject spearHitbox, airAttackHitBox, player;
    Vector3 playerPos, targetPos, vfxPos;
    private NavMeshAgent agent;
    public float movementSpeed = 5, distToPlayer, attackingMovementSpeed = 0;
    public float normalAttackRange;
    public float bossTimer = 16f, maxTimer = 16f, normalAttackTimer = 5f, normalAttackMaxTimer = 5f,bossCurrentHP, bossCurrentGauge;
    [SerializeField] bool isCloseToPlayer = false, isFlying = false, isAttacking = false, isBreak = false, canAttack = false, canNormalAttack, playerCollision = false, inCombat, justBreak, dead;
    public AnsonBossHp bossHPSystem;
    public GameObject[] tornadoSpawnPointsPat1, tornadoSpawnPointsPat2;
    public GameObject feathersPos;
    public ParticleSystem feathersvfx;
    public MusicEnum bossMusic, theTree;
    public GameObject death_spawn;

    private BossAttackScript spearAttackScript, airAttackScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //bossHPSystem = GetComponent<AnsonBossHp>();
        player = GameObject.FindGameObjectWithTag("Player");
        spearAttackScript = spearHitbox.GetComponent<BossAttackScript>();
        airAttackScript = airAttackHitBox.GetComponent<BossAttackScript>();
        spearHitbox.SetActive(false);
        airAttackHitBox.SetActive(false);
        spearAttackScript = spearHitbox.GetComponent<BossAttackScript>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        bossTimer = maxTimer;
        isBreak = false;
        agent.stoppingDistance = normalAttackRange;
        death_spawn.SetActive(false);
    }
    
    private void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
        AudioManager.instance.SetMusicArea(bossMusic);
        animator.enabled = true;
        spearHitbox.SetActive(true);
        EnterCombat();
    }

    private void OnDisable()
    {
        animator.enabled = false;
        spearHitbox.SetActive(false);
        
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        
        AudioManager.instance.SetMusicArea(theTree);

        if (bossCurrentHP <= 0){
            death_spawn.SetActive(true);
        }

        ExitCombat();
    }

    private void OnDestroy()
    {
        dead = true;
        
        animator.enabled = false;
        spearHitbox.SetActive(false);
        
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        
        AudioManager.instance.SetMusicArea(theTree);
        death_spawn.SetActive(true);
        ExitCombat();
    }
    
    void OnShadowStart()
    {
        animator.speed = 0.1f;
        //playerShadow = true;
    }

    void OnShadowEnd()
    {
        animator.speed = 1f;
        //playerShadow = false;
    }
    
    private void EnterCombat()
    {
        if (inCombat) return;
        inCombat = true;
        /*attackRange += attackRangeIncrease;
        */
        
        CombatManager.instance.AddEnemyToCombat();
    }

    private void ExitCombat()
    {
        if (!inCombat) return;
        inCombat = false;
        /*attackRange -= attackRangeIncrease;
        */
        
        CombatManager.instance.RemoveEnemyFromCombat();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        bossCurrentHP = bossHPSystem.checkHealth();
        bossCurrentGauge = bossHPSystem.checkGauge();
        targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        vfxPos = new Vector3(transform.position.x, 0, transform.position.z);
        
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!canAttack && !isBreak)
        {
            bossTimer -= Time.deltaTime;
            if (!canNormalAttack)
            {
                normalAttackTimer -= Time.deltaTime;
            }
        }

        if (bossTimer < 0f)
        {
            canAttack = true;
        }

        if (normalAttackTimer < 0f)
        {
            canNormalAttack = true;
        }
        
        if (distToPlayer <= normalAttackRange)
        {
            isCloseToPlayer = true;
        }
        else
        {
            isCloseToPlayer = false;
        }
        
        
        bool canMove = !isAttacking && !isBreak;
        agent.isStopped = !canMove;

        if (canMove)
        {
            agent.SetDestination(playerPos);
        }

        animator.SetBool("isRunning", canMove && agent.velocity.sqrMagnitude > 0.1f);

        if (agent.remainingDistance <= agent.stoppingDistance && !isAttacking)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1.3f);
            }
        }
        
        justBreak = bossHPSystem.checkBreak();
        
        if (justBreak && !isBreak)
        {
            isBreak = true;
            animator.SetTrigger("Break");
            animator.SetBool("BreakStatus", true);
        }
        else if (!justBreak && isBreak)
        {
            isBreak = false;
            animator.SetBool("BreakStatus", false);
            isAttacking = false;
            normalAttackTimer = 5f;
            canNormalAttack = false;
            if (bossTimer < 2f) bossTimer += 3f;
        }
        
        if (isAttacking) { agent.speed = attackingMovementSpeed; }
        else { agent.speed = movementSpeed; }

        if (!isAttacking && isCloseToPlayer && !canAttack && canNormalAttack && !isBreak)
        {
            animator.SetTrigger("AttackPat1");
            canNormalAttack = false;
            normalAttackTimer = normalAttackMaxTimer;
        }
        else if (!isAttacking && canAttack && !isBreak)
        {
            animator.SetTrigger("AttackPat2");
            canAttack = false;
            bossTimer = maxTimer;
            if (normalAttackTimer < 2f) normalAttackTimer += 3f;
        }
        

        if (distToPlayer < 150f)
        {
            EnterCombat();
        }
        else
        {
            ExitCombat();
        }
    }
    
    private void FixedUpdate()
    {
        if (isFlying)
        {
            AirAttackFollow();
        }
    }

    public void SpawnTornadoVFX()
    {
        int num = Random.Range(0, 2);
        switch (num)
        {
            case 0:
                float angle = 0f;
                for(int i = 0; i< tornadoSpawnPointsPat1.Length; i++)
                {
                    Instantiate(TornadoVFX, tornadoSpawnPointsPat1[i].transform.position, Quaternion.Euler(-90f, angle, 0f));
                    angle += 90f;
                }
                break;
            case 1:
                float angle2 = 0f;
                for (int i = 0; i < tornadoSpawnPointsPat2.Length; i++)
                {
                    Instantiate(TornadoVFX, tornadoSpawnPointsPat1[i].transform.position, Quaternion.Euler(-90f, angle2, 0f));
                    angle2 += 90f;
                }
                break;
            default:
                return;
        }
        //TornadoVFX = Instantiate(TornadoVFX, transform.position, Quaternion.identity);

    }

    public void SpawnRockSpikeVFX()
    {
        if (dead) return;
        //RockSpikeVFX = Instantiate(RockSpikeVFX, targetPos, Quaternion.identity);
        RockSpikeVFX.transform.position = targetPos;
        RockSpikeVFX.SetActive(true);

    }

    public void SpawnGroundStompVFX()
    {
        if (dead) return;
        //groundStompVFX = Instantiate(groundStompVFX, vfxPos, Quaternion.identity);
        groundStompVFX.transform.position = vfxPos;
        groundStompVFX.SetActive(true);
        Debug.Log("groundStompVFX Spawned");
    }
    public void EnableAirAttackHitBox()
    {
        if (dead) return;
        airAttackHitBox.SetActive(true);
        Debug.Log("Enabled Air Attack Hitbox");
    }

    public void DisableAirAttackHitBox()
    {
        if (dead) return;
        airAttackHitBox.SetActive(false);
        Debug.Log("Disabled Air Attack Hitbox");
    }

    public void EnableSpearHitBox()
    {
        if (dead) return;
        spearHitbox.SetActive(true);
        Debug.Log("Enabled Spear Attack Hitbox");
    }

    public void DisableSpearHitBox()
    {
        if (dead) return;
        spearHitbox.SetActive(false);
        Debug.Log("Disabled Spear Attack Hitbox");
    }

    public void AirAttackFollow()
    {
        if (dead) return;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 5f);
    }

    public void startFeathersVFX()
    {
        Instantiate(feathersvfx, feathersPos.transform.position, feathersvfx.transform.rotation);
    }

    public void SetIsAttacking()
    {
        isAttacking = !isAttacking;
    }

    public void SetIsFlying()
    {
        isFlying = !isFlying;
    }

    public void EnterBreakState()
    {
        animator.SetBool("BreakStatus", true);
        Debug.Log("Enter Break State");
    }

    public void ExitBreakState()
    {
        animator.SetBool("BreakStatus", false);
        Debug.Log("Exit Break State");
    }

    public void SetAgentSpeed0()
    {
        agent.speed = 0f;
    }

    public void ResetAgentSpeed()
    {
        agent.speed = movementSpeed;
    }

    public void SetPlayerPerfectDodgeTrue()
    {
        if (dead) return;
        spearAttackScript.DodgeWindowTrue();
        airAttackScript.DodgeWindowTrue();
    }
    
    public void SetPlayerPerfectDodgeFalse()
    {
        if (dead) return;
        spearAttackScript.DodgeWindowFalse();
        airAttackScript.DodgeWindowFalse();
    }

    public void PlaySound(string soundName)
    {
        if (dead) return;
        AudioManager.instance.PlayOneShot(soundName, transform.position);
    }
}
