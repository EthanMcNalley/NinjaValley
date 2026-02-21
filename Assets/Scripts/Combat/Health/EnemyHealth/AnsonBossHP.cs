using UnityEngine;

public class AnsonBossHp : EnemyHealth
{
    [Header ("Boss Stuff")]
    [SerializeField] public float maxGauge = 50f;
    [SerializeField] public float maxBreakTimer = 10f;

    public BossHPUI healthBreakBar;
    public float currentGauge;
    public float breakTimer;
    public bool breakState = false;
    private bool initialized = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        currentGauge = maxGauge;
        breakTimer = maxBreakTimer;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBreakStatus();
        if (breakState)
        {
            breakTimer -= Time.deltaTime;
            if (breakTimer <= 0)
            {
                breakTimer = maxBreakTimer;
                breakState = false;
                currentGauge = maxGauge;
            }
        }
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

    public void CheckBreakStatus()
    {
        if (currentGauge <= 0 && !breakState)
        {
            breakState = true;
        }
        if (breakState)
        {
            breakTimer -= Time.deltaTime;
            if (breakTimer <= 0)
            {
                breakTimer = maxBreakTimer;
                breakState = false;
                currentGauge = maxGauge;
            }
        }
    }

    protected override void Dead()
    {
        Debug.Log("Boss Defeated!");
    }
}
