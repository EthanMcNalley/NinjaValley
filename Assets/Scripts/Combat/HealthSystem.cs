using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public ShadowAssassin shadowAssassin;
    private GameObject player;
    public float maxHealthPoint = 20f;
    public float healthPoint = 20f;
    private float shadowMultiplyPersentage;
    private bool playerShadowMode;
    private float damageDuringShadow;
    private float damageBurst;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shadowAssassin = player.GetComponent<ShadowAssassin>();
        healthPoint = maxHealthPoint;
        shadowMultiplyPersentage = shadowAssassin.shadowAssassinDamagePercentage;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        damageDuringShadow = 0f;
    }

    void OnShadowEnd()
    {
        playerShadowMode = false;
        BurstShadowDamage();
    }
    
    public void TakeDamage(float damage)
    {
        healthPoint -= damage;
        Debug.Log(this.name+ " took " + damage + " damage");

        if (playerShadowMode)
        {
            damageDuringShadow += damage;
        }
    }

    void BurstShadowDamage()
    {
        damageBurst = damageDuringShadow * shadowMultiplyPersentage;
        healthPoint -= damageBurst;
        
        Debug.Log(this.name+ " took " + damageBurst + " shadow burst damage");
    }

    public float GetHealthPoint()
    {
        return healthPoint;
    }
}
