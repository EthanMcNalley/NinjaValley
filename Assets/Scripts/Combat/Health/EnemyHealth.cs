using System.Collections;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public ShadowAssassin shadowAssassin;
    private GameObject player;
    private float shadowMultiplyPersentage;
    private bool playerShadowMode;
    private float damageDuringShadow;
    private float damageBurst;
    
    public FloatingHPDisplay healthBar;
    
    void Start()
    {
        healthBar = GetComponentInChildren<FloatingHPDisplay>();
        player = GameObject.FindGameObjectWithTag("Player");
        shadowAssassin = player.GetComponent<ShadowAssassin>();
        shadowMultiplyPersentage = shadowAssassin.shadowAssassinDamagePercentage;
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

    void OnShadowStart()
    {
        playerShadowMode = true;
        shadowMultiplyPersentage = shadowAssassin.shadowAssassinDamagePercentage;
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

        if (playerShadowMode)
        {
            damageDuringShadow += damage;
            damageBurst = damageDuringShadow * shadowMultiplyPersentage;
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
