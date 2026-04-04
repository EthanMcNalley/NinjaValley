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
    [SerializeField] bool isCloseToPlayer = false, isFlying = false, isAttacking = false, isBreak = false, canAttack = false, playerCollision = false;
    public BossHPSystem bossHPSystem;
    public ParticleSystem howlingEffect;
    float speed = 5f, rotationSpeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossHPSystem = GetComponent<BossHPSystem>();
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
        //bossCurrentHP = bossHPSystem.checkHealth();
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
        if (bossCurrentGauge <= 0 && !isBreak)
        {
            isBreak = true;
            animator.SetTrigger("Break");

        }
        else if (bossCurrentGauge > 0)
        {
            isBreak = false;
            animator.SetBool("IsBreak", false);
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


}
