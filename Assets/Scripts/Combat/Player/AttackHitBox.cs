using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class AttackHitBox : MonoBehaviour
{
    private GameObject player;
    public CombatStateManager combatStateManager;
    public CinemachineImpulseSource impulseSource;
    
    private bool isAttacking = false;
    public GameObject katanaTrail;
    public Collider katanaHitbox;
    private TrailRenderer katanaTrailRenderer;
    private MeshRenderer katanaRenderer;
    public Material auraMaterial;

    private HashSet<GameObject> enemyHitted = new HashSet<GameObject>();
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        katanaHitbox = GetComponent<Collider>();
        katanaRenderer = GetComponent<MeshRenderer>();
        katanaTrailRenderer = katanaTrail.GetComponent<TrailRenderer>();
        combatStateManager = player.GetComponent<CombatStateManager>();
        impulseSource = player.GetComponent<CinemachineImpulseSource>();
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
        
        if (!other.gameObject.CompareTag("Enemy")) return;
        
        Debug.Log( "hit " + other.gameObject.name);

        HealthSystem enemy = other.GetComponent<HealthSystem>();
        if (enemy == null) return;
                
        if (enemyHitted.Contains(other.gameObject)) return;
            
        float damage = combatStateManager.GetDamage();
        float charge = combatStateManager.GetShadowCharge();
            
        enemy.TakeDamage(damage);
        //combatStateManager.shadowAssassin.UpdateShadowMeter(charge);
        enemyHitted.Add(other.gameObject);

        if (combatStateManager.currentStateID == AttackData.CombatStateID.GroundAttack3)
        {
            CameraShakeManager.instance.ScreenShakeFromProfile(combatStateManager.ba3ScreenShake, impulseSource);
        }
    }
}
