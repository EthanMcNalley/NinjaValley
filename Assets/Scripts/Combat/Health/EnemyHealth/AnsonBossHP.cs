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
        float outputDamage = damage;
        if (currentGauge > 0)
        {
            currentGauge -= damage;
            healthBreakBar.UpdateBreakBar(currentGauge, maxGauge);
            outputDamage = damage / 0.67f;
            base.TakeDamage(outputDamage);
            
            if (currentGauge < 0)
            {
                currentGauge = 0;
                breakState = true;
            }
        }
        else
        {
            base.TakeDamage(damage);
        }
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
    
    protected override void OnShadowStart()
    {
        base.OnShadowStart();
        if (breakState)
        {
            breakTimer = Mathf.Max(0f, breakTimer - shadowAssassin.shadowAssassinDuration);
        }
    }

    public void CheckBreakStatus()
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
        tenguBoss.enabled = false;
        base.Dead();
    }
}
