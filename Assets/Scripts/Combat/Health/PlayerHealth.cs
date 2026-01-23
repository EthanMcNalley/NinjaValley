using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : HealthSystem
{
    public Slider healthBarSlider;
    private Image healthFillImage;
    public Color maxColor = Color.green;
    public Color minColor = Color.red;
    public float health_pickup_value;
    private float ratio;
    private float adjustedRatio;

    void Start()
    {
        healthFillImage = healthBarSlider.fillRect.GetComponent<Image>();
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        // ratio = Mathf.Clamp01(currHealthPoint / maxHealthPoint);
        // healthBarSlider.value = ratio;
        
        // adjustedRatio = Mathf.Pow(ratio, 1.3f);
        // healthFillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);
    }

    void Update()
    {
        ratio = Mathf.Clamp01(currHealthPoint / maxHealthPoint);
        healthBarSlider.value = ratio;
        
        adjustedRatio = Mathf.Pow(ratio, 1.3f);
        healthFillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);
    }
    
    protected override void Dead()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            if (currHealthPoint < maxHealthPoint)
            {
                currHealthPoint = currHealthPoint + health_pickup_value;

                if (currHealthPoint > maxHealthPoint)
                {
                    currHealthPoint = maxHealthPoint;
                }
            } 
        }
    }
}
