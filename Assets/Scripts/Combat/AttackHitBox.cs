using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    public CombatStateManager CombatStateManager;

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
