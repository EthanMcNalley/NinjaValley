using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : HealthSystem
{
    public Slider healthBarSlider;
    private Image healthFillImage;
    public Color maxColor = Color.green;
    public Color minColor = Color.red;
    public float health_pickup_value;
    public Volume hurtVolume;
    public float fadeSpeed = 1f;
    private float ratio;
    private float adjustedRatio;

    void Start()
    {
        healthFillImage = healthBarSlider.fillRect.GetComponent<Image>();
        hurtVolume.weight = 0f;
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        hurtVolume.weight = 1f;
        
        //StopCoroutine(HurtVolume());
        //StartCoroutine(HurtVolume());
    }
    
    /*private IEnumerator HurtVolume()
    {
        for (float weight = 1f; weight >= 0; weight -=  0.05f)
        {
            hurtVolume.weight = weight;
            yield return new WaitForSeconds(0.05f);
        }
        
        hurtVolume.weight = 0f;
    }*/

    void Update()
    {
        ratio = Mathf.Clamp01(currHealthPoint / maxHealthPoint);
        healthBarSlider.value = ratio;
        
        adjustedRatio = Mathf.Pow(ratio, 1.3f);
        healthFillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);

        if (hurtVolume.weight > 0f)
        {
            hurtVolume.weight = Mathf.MoveTowards(hurtVolume.weight, 0f, Time.deltaTime * fadeSpeed);
        }
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
