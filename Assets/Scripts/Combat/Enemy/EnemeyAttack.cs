using System;
using UnityEngine;

public class EnemeyAttack : MonoBehaviour
{
    public float damage;
    private GameObject player;
    private HealthSystem PlayerHealth;
    private bool attacked = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = player.GetComponent<HealthSystem>();
    }

    void OnDisable()
    {
        attacked = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !attacked)
        {
            PlayerHealth.TakeDamage(damage);
            attacked = true;
        }
    }
}
