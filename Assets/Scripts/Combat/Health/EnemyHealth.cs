using System.Collections;
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

        if (playerShadowMode)
        {
            damageDuringShadow += damage;
            damageBurst = damageDuringShadow * shadowMultiplyPercentage;
        }
        healthBar.UpdateHealthBar(currHealthPoint, maxHealthPoint, damageBurst);
        
    }

    protected override void Dead()
    {
        Destroy(gameObject);
    }
    

    void BurstShadowDamage()
    {
        currHealthPoint -= damageBurst;
        healthBar.UpdateShadowBar(currHealthPoint, maxHealthPoint);
        
        damageDuringShadow = 0f;
        damageBurst = 0f;
        Debug.Log(this.name+ " took " + damageBurst + " shadow burst damage");
    }
}
