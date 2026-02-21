using UnityEngine;

public class BossAttackScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage;
    private GameObject player;
    private HealthSystem PlayerHealth;
    private NewMovement newMovement;
    private CombatStateManager combatStateManager;
    private bool attacked = false;
    public bool dodgeWindow = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        newMovement = player.GetComponent<NewMovement>();
        PlayerHealth = player.GetComponent<HealthSystem>();
        combatStateManager = player.GetComponent<CombatStateManager>();
    }

    void OnDisable()
    {
        attacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !attacked)
        {
            if (PlayerHealth.isInvincible) return;
            Vector3 hitDirection = (other.transform.position - transform.position);
            newMovement.KnockbackPlayer(hitDirection,2f,1f);
            PlayerHealth.TakeDamage(damage);
            attacked = true;
        }
        
        /*if (other.CompareTag("Player") && dodgeWindow)
        {
            combatStateManager.SetPerfectDodgeWindow(true);
        }
        
        if (other.CompareTag("Player") && !dodgeWindow)
        {
            combatStateManager.SetPerfectDodgeWindow(false);
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && dodgeWindow)
        {
            combatStateManager.SetPerfectDodgeWindow(true);
        }
        
        if (other.CompareTag("Player") && !dodgeWindow)
        {
            combatStateManager.SetPerfectDodgeWindow(false);
        }
    }
    
    public void DodgeWindowTrue()
    {
        dodgeWindow = true;
    }
    
    public void DodgeWindowFalse()
    {
        dodgeWindow = false;
    }
}
