using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField]
    private float damage;
    public float attackCooldown = 0.3f;
    private bool isAttack=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttack = true;
            
        }

        
        
    }

    private void OnTriggerStay(Collider other)
    {   
        if (isAttack)
        {
            if (other.gameObject.tag =="Enemy")
            {
                HealthSystem enemy = other.GetComponent<HealthSystem>();
                enemy.TakeDamage(damage);
                isAttack = false;
            }
        }
    }
}
