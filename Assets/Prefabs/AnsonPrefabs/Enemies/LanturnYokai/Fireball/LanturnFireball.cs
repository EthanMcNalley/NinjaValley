using UnityEngine;

public class LanturnFireball : MonoBehaviour
{
    public float damage;
    public float startSpeed;
    [SerializeField]private float fireballSpeed;
    public float duration = 10f;
    public float slowSpeed = 5f;
    
    private Rigidbody fireballRB;
    private Animator fireballAnimator;
    private TimeManager.TimeState timeState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireballRB = GetComponent<Rigidbody>();
        fireballAnimator = GetComponent<Animator>();
        fireballSpeed = startSpeed;
        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED &&  timeState == TimeManager.TimeState.NORMAL)
        {
            fireballSpeed = slowSpeed;
            fireballAnimator.speed = 0.1f;
            timeState = TimeManager.TimeState.SLOWED;
        }
        else if (TimeManager.time_state == TimeManager.TimeState.NORMAL && timeState == TimeManager.TimeState.SLOWED)
        {
            fireballSpeed = startSpeed;
            fireballAnimator.speed = 1f;
            timeState = TimeManager.TimeState.NORMAL;
        }
    }
    
    void FixedUpdate()
    {
        fireballRB.MovePosition((fireballRB.position + fireballSpeed * slowSpeed * Time.fixedDeltaTime * transform.forward));
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
