using UnityEngine;

public class BossHPSystem : MonoBehaviour
{
    [SerializeField] public float maxHP = 100f;
    [SerializeField] public float maxGauge = 50f;
    [SerializeField] public float maxBreakTimer = 10f;

    public float currentHP;
    public float currentGauge;
    public float breakTimer;
    public bool breakState = false;
    private bool initialized = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        currentHP = maxHP;
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

        /*if (breakState)
        {
            currentGauge += Time.deltaTime * 5f;
            if (currentGauge >= maxGauge)
            {
                currentGauge = maxGauge;
                breakState = false;
            }
        }*/
        if (currentHP <= 0)
        {
            //Destroy(this.gameObject);
            Debug.Log("Boss Defeated!");
        }
    }
    public void TakeDamage(float damage)
    {
        if (currentGauge > 0)
        {
            currentGauge -= damage;
            currentHP -= (damage / 0.67f);
            if (currentGauge < 0)
            {
                currentGauge = 0;
            }
        }
        else
        {
            currentHP -= damage;
            if (currentHP < 0)
            {
                currentHP = 0;
            }
        }
    }

    public float checkHealth()
    {
        return currentHP;
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
}
