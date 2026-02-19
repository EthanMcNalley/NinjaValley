using System.Xml;
using UnityEngine;
using UnityEngine.AI;

public class TenguBoss : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject TornadoVFX, RockSpikeVFX, groundStompVFX;
    [SerializeField] GameObject spearHitbox, airAttackHitBox, player;
    Vector3 playerPos, targetPos, vfxPos;
    private NavMeshAgent agent;
    public float movementSpeed = 5, distToPlayer, attackingMovementSpeed = 0;
    public float bossTimer = 20f, maxTimer = 20f, bossCurrentHP, bossCurrentGauge;
    [SerializeField] bool isCloseToPlayer = false, isFlying = false, isAttacking = false, isBreak = false, canAttack = false, playerCollision = false;
    public AnsonBossHP bossHPSystem;
    public GameObject[] tornadoSpawnPointsPat1, tornadoSpawnPointsPat2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossHPSystem = GetComponent<AnsonBossHP>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        spearHitbox.SetActive(false);
        airAttackHitBox.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        bossTimer = maxTimer;

    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        //bossCurrentHP = bossHPSystem.checkHealth();
        //bossCurrentGauge = bossHPSystem.checkGauge();
        targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        vfxPos = new Vector3(transform.position.x, 0, transform.position.z);
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!canAttack) { bossTimer -= Time.deltaTime; }

        if (bossTimer < 0f) { canAttack = true; }


        if (!isAttacking && !isBreak&& !playerCollision)
        {
            agent.SetDestination(playerPos);
            animator.SetBool("isRunning", true);
        }
        else if(!isAttacking && !isBreak && playerCollision)
        {
            animator.SetBool("isRunning", false);
            agent.ResetPath();
        }
        if (bossCurrentGauge <= 0 && !isBreak)
        {
            isBreak = true;
            animator.SetTrigger("Break");

        }
        else if (bossCurrentGauge > 0)
        {
            isBreak = false;
            animator.SetBool("BreakStatus", false);
        }
        if (isAttacking) { agent.speed = attackingMovementSpeed; }
        else { agent.speed = movementSpeed; }

        if (!isAttacking && isCloseToPlayer && canAttack && bossHPSystem.currentGauge > 0)
        {
            animator.SetTrigger("AttackPat1");
            canAttack = false;
            bossTimer = 20f;
        }
        else if (!isAttacking && !isCloseToPlayer && canAttack && bossHPSystem.currentGauge > 0)
        {
            animator.SetTrigger("AttackPat2");
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
        if (distToPlayer <= 7.5f)
        {
            isCloseToPlayer = true;
        }
        else
        {
            isCloseToPlayer = false;
        }

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
        RockSpikeVFX = Instantiate(RockSpikeVFX, targetPos, Quaternion.identity);
    }

    public void SpawnGroundStompVFX()
    {
        groundStompVFX = Instantiate(groundStompVFX, vfxPos, Quaternion.identity);
    }
    public void EnableAirAttackHitBox()
    {
        airAttackHitBox.SetActive(true);
    }

    public void DisableAirAttackHitBox()
    {
        airAttackHitBox.SetActive(false);
    }

    public void EnableSpearHitBox()
    {
        spearHitbox.SetActive(true);
    }

    public void DisableSpearHitBox()
    {
        spearHitbox.SetActive(false);
    }

    public void AirAttackFollow()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 5f);
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
        if(collision.gameObject.tag == "Player")
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
