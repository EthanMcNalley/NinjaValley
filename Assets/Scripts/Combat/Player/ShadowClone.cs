using System;
using UnityEngine;

public class ShadowClone : MonoBehaviour
{
    private CombatStateManager combatStateManager;
    private GameObject player;
    private Animator animator;
    public float cloneDuration = 6f;
    public float damageMultiplier = 0.4f;
    [SerializeField] private float currentDamage;
    public GameObject hitBoxGO;
    private GeneralAttackHitbox hitbox;

    public float enemyDetectRadius = 50f;
    public LayerMask enemyLayer;
    private GameObject closestEnemy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatStateManager = player.GetComponent<CombatStateManager>();
        animator = GetComponent<Animator>();
        hitbox = hitBoxGO.GetComponent<GeneralAttackHitbox>();
        
        combatStateManager.PlayerAttack += OnPlayerAttack;
    }
    
    void OnDisable()
    {
        if (combatStateManager != null)
        {
            combatStateManager.PlayerAttack -= OnPlayerAttack;
        }
    }

    void OnPlayerAttack(AttackData data)
    {
        currentDamage = data.damage;
        Vector3 position = data.position;
        Quaternion rotation = data.rotation;
        AttackData.CombatStateID stateID = data.stateID;

        closestEnemy = FindClosest.FindClosestGameObject(transform.position, enemyDetectRadius, enemyLayer);
        Vector3 dir = (closestEnemy.transform.position - transform.position);
        dir.y = 0f;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);

        switch (stateID)
        {
            case AttackData.CombatStateID.GroundAttack1:
                Debug.Log("Clone GroundAttack1");
                animator.SetTrigger("Attack1");
                break;

            case AttackData.CombatStateID.GroundAttack2:
                Debug.Log("Clone GroundAttack2");
                animator.SetTrigger("Attack2");
                break;

            case AttackData.CombatStateID.GroundAttack3:
                Debug.Log("Clone GroundAttack3");
                animator.SetTrigger("Attack1");
                break;

            default:
                Debug.Log("Clone fail to find attack state");
                break;
        }
    }

    public void StartAttack()
    {
        hitbox.BeginAttack(currentDamage * damageMultiplier);
    }

    public void EndAttack()
    {
        hitbox.EndAttack();
    }

    // Update is called once per frame
    void Update()
    {
        cloneDuration -= Time.deltaTime;
        if (cloneDuration <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
