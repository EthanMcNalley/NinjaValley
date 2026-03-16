using UnityEngine;

public class AnsonBossHp : EnemyHealth
{
    [Header ("Boss Stuff")]
    [SerializeField] public float maxGauge = 50f;
    [SerializeField] public float maxBreakTimer = 10f;

    public BossHPUI healthBreakBar;
    public TenguBoss tenguBoss;
    public float currentGauge;
    public float breakTimer;
    public bool breakState = false;
    private bool initialized = false;

    public float healthSpawnThreshold = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        currentGauge = maxGauge;
        breakTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBreakStatus();
    }
    
    public override void TakeDamage(float damage)
    {
        TakeDamage(damage, DamageSource.Other);
    }
    
    public override void TakeDamage(float damage, DamageSource source)
    {
        float outputDamage = damage;
        Debug.Log("Damage dealt using AnsonBossHP");
        if (currentGauge > 0)
        {
            currentGauge -= damage;
            healthBreakBar.UpdateBreakBar(currentGauge, maxGauge);
            outputDamage = damage / 0.67f;
            CheckThreshold(outputDamage);
            base.TakeDamage(outputDamage, source);
            
            if (currentGauge < 0)
            {
                currentGauge = 0;
                breakState = true;
            }
        }
        else
        {
            CheckThreshold(damage);
            base.TakeDamage(damage, source);
        }
        
    }

    private void CheckThreshold(float damage)
    {
        int prevThresholdCount = Mathf.FloorToInt((1f - (currHealthPoint / maxHealthPoint)) / healthSpawnThreshold);
        int currThresholdCount = Mathf.FloorToInt((1f - ((currHealthPoint - damage) / maxHealthPoint)) / healthSpawnThreshold);
        
        //probably not needed since there is almost no possibility of the player going over several thresholds...
        for (int i = prevThresholdCount; i < currThresholdCount; i++)
        {
            Instantiate(health_essence, essence_spawnpoint.transform.position, Quaternion.identity);
        }
    }
    
    protected override void OnShadowStart()
    {
        base.OnShadowStart();
        if (breakState)
        {
            breakTimer = Mathf.Max(0f, breakTimer - shadowAssassin.shadowAssassinDuration);
        }
    }

    private void CheckBreakStatus()
    {
        if (currentGauge <= 0 && !breakState)
        {
            breakState = true;

            if (shadowAssassin == null)
            {
                shadowAssassin = GameObject.FindGameObjectWithTag("Player").GetComponent<ShadowAssassin>();
            }
            
            if (shadowAssassin.getShadowActive())
            {
                breakTimer = Mathf.Max(0f, breakTimer - shadowAssassin.getTimer());
            }
        }
        
        if (breakState)
        {
            breakTimer += Time.deltaTime;
            
            float refill = Mathf.Clamp01(breakTimer / maxBreakTimer) * maxBreakTimer;
            healthBreakBar.UpdateBreakBar(refill, maxBreakTimer);

            if (breakTimer >= maxBreakTimer)
            {
                breakTimer = 0;
                breakState = false;
                currentGauge = maxGauge;
                healthBreakBar.UpdateBreakBar(currentGauge, maxGauge);
            }
        }
    }

    protected override void Dead()
    {
        Debug.Log("Boss Defeated!");
        if (tenguBoss != null)
        {
            tenguBoss.enabled = false;
        }
        base.Dead();
    }
        
    public float checkHealth()
    {
        return currHealthPoint;
    }

    public float checkGauge()
    {
        return currentGauge;
    }

    public bool checkBreak()
    {
        return breakState;
    }
    
    public float checkHealthPercent()
    {
        return currHealthPoint / maxHealthPoint;
    }
}
