using UnityEngine;

public class BossManager : MonoBehaviour
{
    public Animator animator;
    public GameObject player;
    public HealthSystem playerHealth;
    public AnsonBossHp bossHpSystem;
    public bool inCombat;
    public bool isDead;
    public float agroDistance;
    public float distToPlayer;
    
    public BossState currentState;
    public float currentStateTime;

    protected virtual void Start()
    {
        currentState?.EnterState(this);
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
    }
    
    protected virtual void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distToPlayer < agroDistance)
        {
            EnterCombat();
        }
        else
        {
            ExitCombat();
        }
        
        if (currentState == null) return;
        if (inCombat)
        {
            currentStateTime += Time.deltaTime;
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(BossState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        //Reset for new state
        currentStateTime = 0f;
        
        Debug.Log("Switching to current state -> " + currentState);
        
        state.EnterState(this);
    }
    
    
    //General Enemy stuff
    protected virtual void EnterCombat()
    {
        if (inCombat) return;
        inCombat = true;
        
        CombatManager.instance.AddEnemyToCombat();
    }
    
    protected virtual void ExitCombat()
    {
        if (!inCombat) return;
        inCombat = false;
        /*attackRange -= attackRangeIncrease;
        */
        
        CombatManager.instance.RemoveEnemyFromCombat();
    }
    
    protected virtual void OnShadowStart()
    {
        if (animator != null) animator.speed = 0.1f;
        //playerShadow = true;
    }

    protected virtual void OnShadowEnd()
    {
        if (animator != null) animator.speed = 1f;
        //playerShadow = false;
    }
    
    protected virtual void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
        
        //AudioManager.instance.SetMusicArea(bossMusic);
    }

    protected virtual void OnDisable()
    {
        animator.enabled = false;
        
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        
        //AudioManager.instance.SetMusicArea(theTree);
        ExitCombat();
    }

    protected virtual void OnDestroy()
    {
        isDead = true;
        
        animator.enabled = false;
        
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        
        //AudioManager.instance.SetMusicArea(theTree);
        ExitCombat();
    }
}
