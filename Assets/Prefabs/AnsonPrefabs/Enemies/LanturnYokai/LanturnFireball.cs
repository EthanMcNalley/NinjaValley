using UnityEngine;

public class LanturnFireball : MonoBehaviour
{
    public float damage;
    public float startSpeed;
    [SerializeField]private float fireballSpeed;
    public float duration = 10f;
    public float slowSpeed = 5f;
    
    private Rigidbody fireballRB;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireballRB = GetComponent<Rigidbody>();
        fireballSpeed = startSpeed;
        Destroy(gameObject, duration);
    }
    
    void FixedUpdate()
    {
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED)
        {
            fireballSpeed = slowSpeed;
        }
        else
        {
            fireballSpeed = startSpeed;
        }
        
      
        fireballRB.MovePosition(fireballRB.position + fireballSpeed * Time.fixedDeltaTime * transform.forward);
        //fireballRB.MoveRotation(Mathf.Sin(30f));
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    }
}
