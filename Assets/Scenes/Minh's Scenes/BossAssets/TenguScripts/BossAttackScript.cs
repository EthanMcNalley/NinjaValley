using UnityEngine;

public class BossAttackScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage;
    private GameObject player;
    private HealthSystem PlayerHealth;
    private NewMovement newMovement;
    private bool attacked = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        newMovement = player.GetComponent<NewMovement>();
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
            Vector3 hitDirection = (other.transform.position - transform.position);
            newMovement.KnockbackPlayer(hitDirection,2f,1f);
            PlayerHealth.TakeDamage(damage);
            attacked = true;
        }
    }
}
