using UnityEngine;

public class LanternYokai : MonoBehaviour
{
    public GameObject player;
    public GameObject fireBallPrefab;
    public Animator animator;
    
    public float attackRange = 50f;
    public float attackRangeIncrease = 15f;
    public float fireballCooldown = 3f;
    public float fireballTimer;
    public GameObject shootPos;
    private bool inCombat = false, isShadow;
    
    public float distToPlayer;
    private bool playerShadow;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distToPlayer < attackRange && !inCombat)
        {
            EnterCombat();
        }
        else if (distToPlayer >= attackRange && inCombat)
        {
            ExitCombat();
        }
    }

    void LateUpdate()
    {
        if (inCombat && !playerShadow) //remove playerShadow maybe if I want to fix this
        {
            AttackCheck();
            transform.LookAt(player.transform,  Vector3.up);
        }
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
        animator.speed = 0.1f;
        isShadow = true;
        playerShadow = true;
    }

    void OnShadowEnd()
    {
        animator.speed = 1f;
        isShadow = false;
        playerShadow = false;
    }
    
    private void EnterCombat()
    {
        if (inCombat) return;
        attackRange += attackRangeIncrease;
        inCombat = true;
        
        CombatManager.instance.AddEnemyToCombat();
    }

    private void ExitCombat()
    {
        if (!inCombat) return;
        attackRange -= attackRangeIncrease;
        inCombat = false;
        
        CombatManager.instance.RemoveEnemyFromCombat();
    }

    private void AttackCheck()
    {
        fireballTimer -= isShadow? Time.deltaTime: Time.deltaTime * 0.1f;

        if (fireballTimer <= 0f)
        {
            animator.SetTrigger("Shoot");
            fireballTimer = fireballCooldown;
        }
    }
    
    public void Shoot()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(fireBallPrefab, shootPos.transform.position, lookRotation);
    }
}
