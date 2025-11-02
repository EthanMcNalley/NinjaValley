using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField]
    private float damage;
    public float attackCooldown = 0.3f;
    private bool isAttacking = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
                HealthSystem enemy = other.GetComponent<HealthSystem>();
                enemy.TakeDamage(damage);
        }
    }
}
