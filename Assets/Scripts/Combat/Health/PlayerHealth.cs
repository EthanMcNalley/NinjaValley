using System;
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
    [SerializeField] Volume death_volume;
    [SerializeField] GameObject game_over_UI;

    void Start()
    {
        game_over_UI.SetActive(false);
        healthFillImage = healthBarSlider.fillRect.GetComponent<Image>();
        hurtVolume.weight = 0f;
        death_volume.weight = 0.0f;
    }
    
    public override void TakeDamage(float damage)
    {
        TakeDamage(damage, DamageSource.Other);
    }

    public override void TakeDamage(float damage, DamageSource source)
    {
        if (isInvincible)
        {
            return;
        }

        base.TakeDamage(damage, source);
        hurtVolume.weight = 1f;
    }

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
        NewMovement.is_dead = isDead;
        StartCoroutine(Die());
        CombatManager.instance.ResetBoss();
        //SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator Die(){
        death_volume.GetComponent<Animator>().SetTrigger("Death");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2.0f);
        game_over_UI.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            if (currHealthPoint < maxHealthPoint)
            {
                currHealthPoint = Mathf.Clamp(currHealthPoint + health_pickup_value,0, maxHealthPoint);
            } 
        }
    }
}
