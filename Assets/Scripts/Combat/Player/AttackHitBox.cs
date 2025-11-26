using UnityEngine;
using System.Collections.Generic;

public class AttackHitBox : MonoBehaviour
{
    public CombatStateManager CombatStateManager;
    
    private bool isAttacking = false;
    public GameObject katanaTrail;
    public Collider katanaHitbox;
    private TrailRenderer katanaTrailRenderer;

    private HashSet<GameObject> enemyHitted = new HashSet<GameObject>();

    void Start()
    {
        katanaHitbox = GetComponent<Collider>();
        katanaTrailRenderer = katanaTrail.GetComponent<TrailRenderer>();
    }
    
    //For Animation Event
    public void BeginAttack()
    {
        enemyHitted.Clear();
        isAttacking = true;
        katanaHitbox.enabled = true;
        katanaTrailRenderer.emitting = true;
    }

    //For Animation Event
    public void EndAttack()
    {
        isAttacking = false;
        katanaHitbox.enabled = false;
        katanaTrailRenderer.emitting = false;
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
                
            if (enemyHitted.Contains(other.gameObject)) return;
            
            float damage = CombatStateManager.GetDamage();
            float charge = CombatStateManager.GetShadowCharge();
            
            enemy.TakeDamage(damage);
            CombatStateManager.shadowAssassin.UpdateShadowMeter(charge);
            enemyHitted.Add(other.gameObject);
        }
    }
}
