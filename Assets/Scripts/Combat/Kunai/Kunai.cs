using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float damage;
    public float kunaiSpeed;
    public float kunaiTime = 10f;
    
    private Rigidbody kunaiRB;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kunaiRB = GetComponent<Rigidbody>();
        Destroy(gameObject, kunaiTime);
    }

    void FixedUpdate()
    {
        kunaiRB.MovePosition(kunaiRB.position + transform.forward * kunaiSpeed * Time.fixedDeltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HealthSystem enemy = other.GetComponent<HealthSystem>();
            enemy.TakeDamage(damage, HealthSystem.DamageSource.Player);
            
            Destroy(gameObject);
        }
    }
}
