using UnityEngine;

public class BossHPSystem : MonoBehaviour
{
    [SerializeField]
    public float maxHP = 100f, currentHP = 0f;
    public float maxGauge = 50f, currentGauge = 0f;
    public bool breakState = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGauge = maxGauge;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentGauge <= 0)
        {
            breakState = true;
        }
        if(breakState)
        {
           currentGauge += time.deltaTime * 5f;
              if(currentGauge >= maxGauge)
              {
                currentGauge = maxGauge;
                breakState = false;
              }
        }
        if(currentHP <= 0)
        {
            gameObject.destroy(gameObject);
            Debug.Log("Boss Defeated!");
        }
    }
    public void TakeDamage(float damage)
    {
        if (currentGauge > 0)
        {
            currentGauge -= damage;
            currentHP -= (damage / 10);
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
}
