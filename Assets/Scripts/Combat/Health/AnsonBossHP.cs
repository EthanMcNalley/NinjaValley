using UnityEngine;

public class AnsonBossHP : EnemyHealth
{
    [SerializeField] private float maxGauge = 50f;
    [SerializeField] private float maxBreakTimer = 10f;

    public float currentGauge;
    public float breakTimer;
    public bool breakState = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGauge = maxGauge;
        breakTimer = maxBreakTimer;
    }

    // Update is called once per frame
    void Update()
    {
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