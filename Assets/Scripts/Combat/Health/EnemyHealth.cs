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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        }
    }

    protected override void Dead()
    {
        
    }
    

    void BurstShadowDamage()
    {
        damageBurst = damageDuringShadow * shadowMultiplyPersentage;
        healthPoint -= damageBurst;
        
        Debug.Log(this.name+ " took " + damageBurst + " shadow burst damage");
    }
}
