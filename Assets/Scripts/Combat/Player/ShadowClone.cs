using System;
using UnityEngine;

public class ShadowClone : MonoBehaviour
{
    private CombatStateManager combatStateManager;
    private GameObject player;
    private Animator animator;
    public float cloneDuration = 6f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatStateManager = player.GetComponent<CombatStateManager>();
        animator = GetComponent<Animator>();
        combatStateManager.PlayerAttack += OnPlayerAttack;
    }
    
    void OnDisable()
    {
        combatStateManager.PlayerAttack -= OnPlayerAttack;
    }

    void OnPlayerAttack(AttackData data)
    {
        float currentDamage = data.damage;
        Vector3 position = data.position;
        Quaternion rotation = data.rotation;
        AttackData.CombatStateID stateID = data.stateID;

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
