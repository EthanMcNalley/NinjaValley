using UnityEngine;
using System.Collections.Generic;

public class AttackHitBox : MonoBehaviour
{
    public CombatStateManager CombatStateManager;
    
    private bool isAttacking = false;

    private HashSet<GameObject> enemyHitted = new HashSet<GameObject>();
    
    //For Animation Event
    public void BeginAttack()
    {
        isAttacking = true;
        enemyHitted.Clear();
    }

    //For Animation Event
    public void EndAttack()
    {
        isAttacking = false;
        enemyHitted.Clear();
    }
    
    //It's this complicated because somehow if you spin the character so that and sword keeps entering the enemy, you could deal multiple damage with 1 swing
    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthSystem enemy = other.GetComponent<HealthSystem>();
            if (enemy == null) return;
                
            if (enemyHitted.Contains(gameObject)) return;
            
            float damage = CombatStateManager.GetDamage();
            
            enemy.TakeDamage(damage);
            enemyHitted.Add(gameObject);
        }
    }
}
