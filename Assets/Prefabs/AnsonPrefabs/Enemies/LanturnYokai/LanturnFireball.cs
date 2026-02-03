using UnityEngine;

public class LanturnFireball : MonoBehaviour
{
    public float damage;
    public float startSpeed;
    [SerializeField]private float fireballSpeed;
    public float duration = 10f;
    public float slowSpeed = 5f;
    
    public float sinAmplitude = 0.5f;
    public float sinFrequency = 0.5f;
    
    private float slowSwingSpeed;
    
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
            slowSwingSpeed = 8f;
        }
        else
        {
            fireballSpeed = startSpeed;
            slowSwingSpeed = 1f;
        }
        
        float offSet = Mathf.Cos(Time.fixedTime * sinFrequency / slowSwingSpeed) * sinAmplitude;
        
        fireballRB.MovePosition((fireballRB.position + fireballSpeed * Time.fixedDeltaTime * transform.forward) + (offSet * transform.right));
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
