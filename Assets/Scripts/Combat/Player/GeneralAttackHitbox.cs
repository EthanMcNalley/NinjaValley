using UnityEngine;
using System.Collections.Generic;

public class GeneralAttackHitbox : MonoBehaviour
{
    private bool isAttacking = false;
    [SerializeField]private float damage;
    private HashSet<GameObject> enemyHitted = new HashSet<GameObject>();
    public string targetTag = "Enemy";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void BeginAttack(float damage)
    {
        this.damage = damage;
        isAttacking = true;
        gameObject.SetActive(true);
    }
    
    public void EndAttack()
    {
        isAttacking = false;
        enemyHitted.Clear();
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;
        
        if (!other.gameObject.CompareTag(targetTag)) return;
        
        HealthSystem enemy = other.GetComponent<HealthSystem>();
        if (enemy == null) return;
                
        if (enemyHitted.Contains(other.gameObject)) return;
            
        enemy.TakeDamage(damage);
        enemyHitted.Add(other.gameObject);
        
    }
}
