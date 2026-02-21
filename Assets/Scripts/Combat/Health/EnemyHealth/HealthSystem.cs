using UnityEngine;
using UnityEngine.UI;

public abstract class HealthSystem : MonoBehaviour
{
    public float maxHealthPoint = 10f;
    public float currHealthPoint = 10f;
    private bool isDead = false;

    public bool isInvincible;

    public void SetInvincible(bool isInvincible) =>
        this.isInvincible = isInvincible; //just found this is a short version

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHealthPoint = maxHealthPoint;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }

        currHealthPoint = Mathf.Clamp(currHealthPoint - damage, 0f, maxHealthPoint);
        Debug.Log(this.name + " took " + damage + " damage");

        if (currHealthPoint <= 0)
        {
            isDead = true;
            Dead();
        }
    }

    protected abstract void Dead();
}
