using UnityEngine;
using UnityEngine.UI;

public abstract class HealthSystem : MonoBehaviour
{
    public float maxHealthPoint = 100f;
    public float healthPoint = 100f;
    private bool isDead = false;
    protected bool isInvincible { get; private set; }

    public void SetInvincible(bool isInvincible) =>
        this.isInvincible = isInvincible; //just found this is a short version

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthPoint = maxHealthPoint;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }

        healthPoint = Mathf.Clamp(healthPoint - damage, 0f, maxHealthPoint);
        Debug.Log(this.name + " took " + damage + " damage");

        if (healthPoint <= 0)
        {
            isDead = true;
            Dead();
        }
    }

    protected abstract void Dead();
}
