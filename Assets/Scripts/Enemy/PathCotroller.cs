using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PathController : MonoBehaviour
{
    Transform playerPos;
    
    //public PathManager pathManager;
    public float waitTime = 2.5f;
    Waypoint targetPoints;
    private NavMeshAgent agent;
    public float movementSpeed;
    public float RotateSpeed;
    public Animator animator;
    bool isWalking;
    public float distToPlayer;
    public float chaseDistance = 10f;
    public float chaseIncreaseDistance = 10f;
    public GameObject player, target;
    public GameObject hitBox;
    private CombatStateManager combatStateManager;
    private bool canMove = true, isAttacking = false; 
    [SerializeField]private bool isChasing = false;
    public GameObject[] patrolPoints;
    public int prevIndex = -1;
    public Transform rotateNode;
    
    public float shadowSlow = 0.1f;
    private bool attackOnCooldown;
    public bool dodgeWindow = false;
    
    private bool inCombat = false;
    public bool tutorial;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (patrolPoints.Length > 0)
        {
            target = patrolPoints[0];
        }
        player = GameObject.FindGameObjectWithTag("Player");
        combatStateManager = player.GetComponent<CombatStateManager>();
        agent.speed = movementSpeed;
    }

    private void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
    }

    private void OnDisable()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        
        ExitCombat();
    }

    private void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        
        ExitCombat();
    }

    void OnShadowStart()
    {
        //animator.SetFloat("Speed", shadowSlow);
        animator.speed = shadowSlow;
        agent.speed = movementSpeed * shadowSlow;
        agent.angularSpeed *= shadowSlow;
    }

    void OnShadowEnd()
    {
        //animator.SetFloat("Speed", 1f);
        animator.speed = 1f;
        agent.speed /= shadowSlow;
        agent.angularSpeed /= shadowSlow;
    }
    
    private void EnterCombat()
    {
        if (inCombat) return;
        chaseDistance += chaseIncreaseDistance;
        inCombat = true;
        waitTime = 2.5f;
        canMove = true;
        
        CombatManager.instance.AddEnemyToCombat();
    }

    private void ExitCombat()
    {
        if (!inCombat) return;
        chaseDistance -= chaseIncreaseDistance;
        inCombat = false;
        
        CombatManager.instance.RemoveEnemyFromCombat();
    }
    
    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        bool wasChasing = isChasing;
        isChasing = distToPlayer <= chaseDistance;
        
        if (!wasChasing && isChasing)
        {
            target = player;
        }
        else if (wasChasing && !isChasing && patrolPoints.Length > 0)
        {
            target = patrolPoints[prevIndex >= 0 ? prevIndex : 0];
            agent.ResetPath();
        }
        
        if (canMove && !isAttacking && patrolPoints.Length > 0)
        {
            agent.destination = target.transform.position;
            animator.SetBool("Moving", true);
        }

        if(!canMove && !isChasing)
        {
            animator.SetBool("Moving", false);
            waitTime -= Time.deltaTime;
            if (waitTime <= 0 && patrolPoints.Length > 0)
            {
                int a = Random.Range(0, patrolPoints.Length);

                // keep picking until it's not the same as prevIndex
                while (a == prevIndex && patrolPoints.Length > 1)
                {
                    a = Random.Range(0, patrolPoints.Length);
                }

                prevIndex = a;
                target = patrolPoints[a];
                canMove = true;
                waitTime = 2.5f;
            }
        }

        if (!wasChasing && isChasing)
        {
            EnterCombat();
        }
        if (wasChasing && !isChasing)
        {
            ExitCombat();
        }
        
        //rotateTowardsTarget();
    }


    /*void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }*/

    public void Attack()
    {
        animator.SetBool("Attack", true);
        agent.speed = 0;
    }

    public void HitBoxOn()
    {
        hitBox.SetActive(true);
    }

    public void HitBoxOff()
    {
        hitBox.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tutorial)
        {
            EnterCombat();
            Attack();
            isAttacking = true;
        }
        
        //Debug.Log(other.name);
        if (!isChasing && other.CompareTag("Point") && patrolPoints.Length > 0)
        {   
            //canMove = false;
            for (int i = 0; i < patrolPoints.Length; i++){
                if (other.gameObject == patrolPoints[i] && target == patrolPoints[i])
                {
                    canMove = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isAttacking && !tutorial)
        {
            Attack();
            isAttacking = true;
        }

        if (other.CompareTag("Player") && dodgeWindow)
        {
            combatStateManager.SetPerfectDodgeWindow(true);
        }
        
        if (other.CompareTag("Player") && !dodgeWindow)
        {
            combatStateManager.SetPerfectDodgeWindow(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatStateManager.SetPerfectDodgeWindow(false);
        }
    }

    public void checkAttack()
    {
        isAttacking = false;
        agent.speed = movementSpeed;
        animator.SetBool("Attack", false);
    }

    public void DodgeWindowTrue()
    {
        dodgeWindow = true;
    }
    
    public void DodgeWindowFalse()
    {
        dodgeWindow = false;
    }

    public void SetIsChasing()
    {
        isChasing = true;
    }
}
