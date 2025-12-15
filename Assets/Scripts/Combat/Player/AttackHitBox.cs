using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AttackHitBox : MonoBehaviour
{
    public CombatStateManager CombatStateManager;
    
    private bool isAttacking = false;
    public GameObject katanaTrail;
    public Collider katanaHitbox;
    private TrailRenderer katanaTrailRenderer;
    private MeshRenderer katanaRenderer;
    public Material auraMaterial;

    private HashSet<GameObject> enemyHitted = new HashSet<GameObject>();
    
    void Start()
    {
        katanaHitbox = GetComponent<Collider>();
        katanaRenderer = GetComponent<MeshRenderer>();
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
    
    private void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
    }

    private void OnDisable()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
    }

    private void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
    }

    void OnShadowStart()
    {
        Material[] materials = katanaRenderer.materials;
        Array.Resize(ref materials, materials.Length + 1);
        materials[^1] = auraMaterial;
        
        katanaRenderer.materials = materials;
    }

    void OnShadowEnd()
    {
        Material[] materials = katanaRenderer.materials;
        Array.Resize(ref materials, materials.Length - 1);
        
        katanaRenderer.materials = materials;
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
