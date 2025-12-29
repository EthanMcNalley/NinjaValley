using System;
using UnityEngine;

public class ShadowClone : MonoBehaviour
{
    private CombatStateManager combatStateManager;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatStateManager = player.GetComponent<CombatStateManager>();
        combatStateManager.PlayerAttack += OnPlayerAttack;
    }
    
    void OnDisable()
    {
        combatStateManager.PlayerAttack -= OnPlayerAttack;
    }

    void OnPlayerAttack()
    {
        
    }
    


    // Update is called once per frame
    void Update()
    {
        
    }
}
