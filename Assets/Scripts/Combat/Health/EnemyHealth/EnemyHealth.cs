using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyHealth : HealthSystem
{
    public ShadowAssassin shadowAssassin;
    private GameObject player;
    public Animator animator;
    private float shadowMultiplyPercentage;
    private bool playerShadowMode;
    private float damageDuringShadow;
    private float damageBurst;
    private AttackData playerAttackData;
    
    public EnemyHPUI healthBar;
    public VisualEffect particles;
    public VisualEffect normalDeathParticles;
    public VisualEffect executeDeathParticles;
    public GameObject particalTransform;
    public GameObject health_essence;
    public GameObject shadow_essence;
    public GameObject essence_spawnpoint;
    private VisualEffect particlesInstance;
    public GameObject executeOutline, cross;
    
    public EnemySoul enemySoul;
    public GameObject soul;
    
    public GameObject baseGameObjectDestroy;
    
    [Header("Sounds")]
    public EventReference hurtSound;
    public EventReference deathSound;
    
    private bool markedForDeath = false;
    private bool markedForExecute = false;
    public bool isBoss = false;

    void Awake()
    {
        //healthBar = GetComponentInChildren<EnemyHPUI>();
        if (animator == null) animator = GetComponent<Animator>();
        if (cross != null) { cross.SetActive(false);}
        if (executeOutline != null) { executeOutline.SetActive(false);}
        if (soul != null) { soul.SetActive(false);}
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shadowAssassin = player.GetComponent<ShadowAssassin>();
        shadowMultiplyPercentage = shadowAssassin.shadowAssassinDamagePercentage;
    }

    protected virtual void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
        CombatStateManager.PlayerAttack += OnPlayerAttack;
    }

    void OnDisable()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        CombatStateManager.PlayerAttack -= OnPlayerAttack;
    }

    private void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        CombatStateManager.PlayerAttack -= OnPlayerAttack;
    }

    protected virtual void OnPlayerAttack(AttackData data)
    {
        playerAttackData = data;
    }
    
    protected virtual void OnShadowStart()
    {
        if (shadowAssassin == null || player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            shadowAssassin = player.GetComponent<ShadowAssassin>();
        }
        
        playerShadowMode = true;
        markedForDeath = false;
        if (soul != null) 
        {
            soul.SetActive(true);
            enemySoul.SetShadow(true);
        }
        shadowMultiplyPercentage = shadowAssassin.shadowAssassinDamagePercentage;
        damageDuringShadow = 0f;
        damageBurst = 0f;
    }

    protected virtual void OnShadowEnd()
    {
        playerShadowMode = false;
        BurstShadowDamage();

        if (markedForExecute)
        {
            ShadowExecute();
        }
        
        markedForDeath = false;
        if (soul != null)
        {
            enemySoul.SetShadow(false);
            soul.SetActive(false);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage, DamageSource.Other);
    }
    
    public override void TakeDamage(float damage, DamageSource source)
    {
        base.TakeDamage(damage, source);
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        
        particlesInstance =  Instantiate(particles, particalTransform.transform.position, Quaternion.identity);
        
        if (!hurtSound.IsNull)
        {
            AudioManager.instance.PlayOneShot(hurtSound, transform.position);
        }

        if (playerShadowMode)
        {
            damageDuringShadow += damage;
            damageBurst = damageDuringShadow * shadowMultiplyPercentage;
            
            if (!markedForDeath)
            {
                markedForDeath = true;
                if (enemySoul != null) {enemySoul.SetMarkedForDeath(true);}
            }
        }
        else if (!playerShadowMode && playerAttackData.stateID == AttackData.CombatStateID.GroundAttack3 && source == DamageSource.Player)
        {
            Instantiate(shadow_essence, essence_spawnpoint.transform.position, Quaternion.identity);
        }

        if ((currHealthPoint - damageBurst) <= 0 && playerShadowMode)
        {
            markedForExecute =  true;
            if (enemySoul != null) {enemySoul.SetMarkedForExecute(true);}
            if (cross != null) { cross.SetActive(true); }
            if (executeOutline != null) { executeOutline.SetActive(true); }
            FreezeEnemy(true);
        }
        
        healthBar.UpdateHealthBar(currHealthPoint, maxHealthPoint, damageBurst);
    }

    protected override void Dead()
    {
        if (playerShadowMode)
        {
            markedForExecute = true;
            FreezeEnemy(true);
            
            return;
        }

        if (markedForExecute)
        {
            return;
        }
        
        particlesInstance =  Instantiate(normalDeathParticles, particalTransform.transform.position, Quaternion.identity);
        if (!deathSound.IsNull)
        {
            AudioManager.instance.PlayOneShot(deathSound, transform.position);
        }
        Instantiate(health_essence, essence_spawnpoint.transform.position, Quaternion.identity);

        if (baseGameObjectDestroy != null && !isBoss)
        {
            Destroy(baseGameObjectDestroy, 0);
        }
        else if (!isBoss)
        {
            Destroy(gameObject, 0);
        }
        
    }

    public void FreezeEnemy(bool freeze)
    {
        if (animator != null)
        {
            animator.speed = freeze ? 0f : 1f;
        }
    }

    void ShadowExecute()
    {
        particlesInstance =  Instantiate(executeDeathParticles, particalTransform.transform.position, Quaternion.identity);
        if (!deathSound.IsNull)
        {
            AudioManager.instance.PlayOneShot(deathSound, transform.position);
        }
        Destroy(gameObject, 0);
    }
    
    void BurstShadowDamage()
    {
        if (damageBurst ==  0) return;
        currHealthPoint -= damageBurst;
        healthBar.UpdateShadowBar(currHealthPoint, maxHealthPoint);
        
        Debug.Log(this.name+ " took " + damageBurst + " shadow burst damage");
        damageDuringShadow = 0f;
        damageBurst = 0f;
    }
    
    public float ReturnHealthPoint()
    {
        return currHealthPoint;
    }
}
