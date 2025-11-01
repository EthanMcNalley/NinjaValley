using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField]
    private float damage;
    public float attackCooldown = 0.3f;
    private bool isAttack=false;

    private void OnTriggerStay(Collider other)
    {   
        if (isAttack && other.gameObject.CompareTag("Enemy"))
        {
                HealthSystem enemy = other.GetComponent<HealthSystem>();
                enemy.TakeDamage(damage);
                isAttack = false;
        }
    }
}
