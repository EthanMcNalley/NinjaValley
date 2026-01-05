using System.Collections;
using FMODUnity;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public ShadowAssassin shadowAssassin;
    private GameObject player;
    private Animator animator;
    private float shadowMultiplyPercentage;
    private bool playerShadowMode;
    private float damageDuringShadow;
    private float damageBurst;
    
    public FloatingHPDisplay healthBar;
    public ParticleSystem particles;
    public ParticleSystem normalDeathParticles;
    public ParticleSystem executeDeathParticles;
    public GameObject particalTransform;
    public GameObject health_essence;
    public GameObject shadow_essence;
    public GameObject essence_spawnpoint;
    private ParticleSystem particlesInstance;
    
    public EnemySoul enemySoul;
    public GameObject soul;
    
    [Header("Sounds")]
    public EventReference hurtSound;
    public EventReference deathSound;
    
    private bool markedForDeath = false;
    private bool markedForExecute = false;
    
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHPDisplay>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        shadowAssassin = player.GetComponent<ShadowAssassin>();
        shadowMultiplyPercentage = shadowAssassin.shadowAssassinDamagePercentage;
        soul.SetActive(false);
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
    }

    private void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
    }
    
    void OnShadowStart()
    {
        playerShadowMode = true;
        markedForDeath = false;
        soul.SetActive(true);
        enemySoul.SetShadow(true);
        shadowMultiplyPercentage = shadowAssassin.shadowAssassinDamagePercentage;
        damageDuringShadow = 0f;
        damageBurst = 0f;
    }

    void OnShadowEnd()
    {
        playerShadowMode = false;
        BurstShadowDamage();

        if (markedForExecute)
        {
            ShadowExecute();
        }
        
        markedForDeath = false;
        enemySoul.SetShadow(false);
        soul.SetActive(false);
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
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
                enemySoul.SetMarkedForDeath(true);
            }
        }
        else
        {
            //Instantiate(shadow_essence, essence_spawnpoint.transform.position, Quaternion.identity);
        }

        if ((currHealthPoint - damageBurst) <= 0)
        {
            markedForExecute =  true;
            enemySoul.SetMarkedForExecute(true);
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
        Destroy(gameObject);
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
        Destroy(gameObject);
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

    
    
    void SoulUI()
    {
        
    }
}
