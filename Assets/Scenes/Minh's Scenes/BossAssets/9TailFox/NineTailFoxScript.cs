using UnityEngine;
using UnityEngine.AI;

public class NineTailFoxScript : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject BeamVFX, groundStompVFX;
    [SerializeField] GameObject beamHitbox,howlingHitbox, airAttackHitBox, player;
    Vector3 playerPos, targetPos, vfxPos;
    private NavMeshAgent agent;
    public float movementSpeed = 5, distToPlayer, attackingMovementSpeed = 0;
    public float bossTimer = 20f, maxTimer = 20f, bossCurrentHP, bossCurrentGauge;
    [SerializeField] bool isCloseToPlayer = false, isFlying = false, isAttacking = false, isBreak = false, canAttack = false, playerCollision = false, inCombat, justBreak, dead;
    public AnsonBossHp bossHPSystem;
    public ParticleSystem howlingEffect;
    float speed = 5f, rotationSpeed = 10f;
    public MusicEnum bossFoxMusic, finalArea;
    public GameObject death_spawn;
    private BossAttackScript beamAttackScript, howlingAttackScript, airAttackScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossHPSystem = GetComponent<AnsonBossHp>();
        checkingHitBoxWhenPlay();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        airAttackHitBox.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        bossTimer = maxTimer;
        BeamVFX.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        bossCurrentHP = bossHPSystem.checkHealth();
        bossCurrentGauge = bossHPSystem.checkGauge();
        targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        vfxPos = new Vector3(transform.position.x, 0, transform.position.z);
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!canAttack) { bossTimer -= Time.deltaTime; }

        if (bossTimer < 0f) { canAttack = true; }


        if (!isAttacking && !isBreak && !playerCollision)
        {
            //agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        }
        else if (!isAttacking && !isBreak && playerCollision)
        {
            animator.SetBool("isWalking", false);
            //agent.ResetPath();
        }
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
            //normalAttackTimer = 5f;
            //canNormalAttack = false;
            if (bossTimer < 2f) bossTimer += 3f;
        }
        if (isAttacking) 
        {
            //agent.speed = attackingMovementSpeed;
            speed = 0f;
            rotationSpeed = 0f;
            
        }
        else 
        {
            //agent.speed = movementSpeed; 
            speed = 5;
            rotationSpeed = 10f;
        }

        if (!isAttacking && isCloseToPlayer && canAttack && bossHPSystem.currentGauge > 0)
        {
            animator.SetTrigger("Attack1");
            canAttack = false;
            bossTimer = 20f;
        }
        else if (!isAttacking && !isCloseToPlayer && canAttack && bossHPSystem.currentGauge > 0)
        {
            animator.SetTrigger("Attack2");
            canAttack = false;
            bossTimer = 20f;
        }

        if (bossHPSystem.currentGauge <= 0 && !bossHPSystem.breakState)
        {
            bossHPSystem.breakState = true;
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
        Vector3 direction = playerPos - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,  rotationSpeed * Time.deltaTime);
        }

            if (distToPlayer <= 2f)
        {
            isCloseToPlayer = true;
        }
        else
        {
            isCloseToPlayer = false;
        }
        if (isFlying && !isCloseToPlayer)
        {
            AirAttackFollow();
        }
        if (isCloseToPlayer)
        {
            isFlying = false;
        }

    }

    public void ActivateBeamVFX()
    {
        BeamVFX.SetActive(true);

    }

    public void DeactivateBeamVFX()
    {
        BeamVFX.SetActive(false);
    }

    public void checkingHitBoxWhenPlay()
    {
        beamHitbox.SetActive(false);
        howlingHitbox.SetActive(false);
        airAttackHitBox.SetActive(false);
    }

   
    public void EnableAirAttackHitBox()
    {
        airAttackHitBox.SetActive(true);
    }

    public void DisableAirAttackHitBox()
    {
        airAttackHitBox.SetActive(false);
    }
    public void EnableHowlingHitBox()
    {
        howlingHitbox.SetActive(true);
        howlingEffect.Play();
    }
    public void DisableHowlingHitBox()
    {
        howlingHitbox.SetActive(false);
    }
    public void EnableBeamHitBox()
    {
        beamHitbox.SetActive(true);
    }

    public void DisableBeamHitBox()
    {
        beamHitbox.SetActive(false);
    }

    public void AirAttackFollow()
    {
        
        //this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 5f);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 15f * Time.deltaTime);
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
    }

    public void ExitBreakState()
    {
        animator.SetBool("BreakStatus", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Enter");
            playerCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Exit");
            playerCollision = false;
        }

    }


    private void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;

        AudioManager.instance.SetMusicArea(bossFoxMusic);
    }

    private void OnDisable()
    {
        animator.enabled = false;

        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;

        AudioManager.instance.SetMusicArea(finalArea);
        death_spawn.SetActive(true);
        ExitCombat();
    }

    private void OnDestroy()
    {
        dead = true;

        animator.enabled = false;

        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;

        AudioManager.instance.SetMusicArea(finalArea);
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
    public void SetPlayerPerfectDodgeTrue()
    {
        if (dead) return;
        beamAttackScript.DodgeWindowTrue();
        howlingAttackScript.DodgeWindowTrue();
        airAttackScript.DodgeWindowTrue();
    }

    public void SetPlayerPerfectDodgeFalse()
    {
        if (dead) return;
        beamAttackScript.DodgeWindowFalse();
        howlingAttackScript.DodgeWindowFalse();
        airAttackScript.DodgeWindowFalse();
    }

    public void PlaySound(string soundName)
    {
        if (dead) return;
        AudioManager.instance.PlayOneShot(soundName, transform.position);
    }




}
