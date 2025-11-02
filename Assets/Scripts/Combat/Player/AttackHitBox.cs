using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public CombatStateManager CombatStateManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthSystem enemy = other.GetComponent<HealthSystem>();
            float damage = CombatStateManager.GetDamage();
            enemy.TakeDamage(damage);
        }
    }
}
