using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyHPUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider shadowBar;
    [SerializeField] protected Image healthFillImage;

    [SerializeField] protected float ratio;

    public virtual void UpdateHealthBar(float currHealth, float maxHealth, float shadowDamage)
    {
        ratio = Mathf.Clamp01((currHealth - shadowDamage) / maxHealth);
        healthBar.value = ratio;

        ratio = Mathf.Clamp01(currHealth / maxHealth);
        shadowBar.value = ratio;
    }

    public virtual void UpdateShadowBar(float currHealth, float maxHealth)
    {
        ratio = Mathf.Clamp01(currHealth / maxHealth);
        shadowBar.value = ratio;
    }
}
