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
    public ParticleSystem deathParticles;
    private ParticleSystem particlesInstance;
    
    [Header("Sounds")]
    public EventReference hurtSound;
    public EventReference deathSound;
    
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHPDisplay>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        shadowAssassin = player.GetComponent<ShadowAssassin>();
        shadowMultiplyPercentage = shadowAssassin.shadowAssassinDamagePercentage;
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
        shadowMultiplyPercentage = shadowAssassin.shadowAssassinDamagePercentage;
        damageDuringShadow = 0f;
        damageBurst = 0f;
    }

    void OnShadowEnd()
    {
        playerShadowMode = false;
        BurstShadowDamage();
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        
        particlesInstance =  Instantiate(particles, transform.position, Quaternion.identity);
        if (!hurtSound.IsNull)
        {
            AudioManager.instance.PlayOneShot(hurtSound, transform.position);
        }

        if (playerShadowMode)
        {
            damageDuringShadow += damage;
            damageBurst = damageDuringShadow * shadowMultiplyPercentage;
        }
        healthBar.UpdateHealthBar(currHealthPoint, maxHealthPoint, damageBurst);
        
    }

    protected override void Dead()
    {
        particlesInstance =  Instantiate(deathParticles, transform.position, Quaternion.identity);
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
        
        damageDuringShadow = 0f;
        damageBurst = 0f;
        Debug.Log(this.name+ " took " + damageBurst + " shadow burst damage");
    }
}
