using UnityEngine;

public class FlyTowardsPlayer : MonoBehaviour
{
    public GameObject player;
    public float speed = 1.0f;
    public float initial_force;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        Vector3 randomDir = new Vector3(
            Random.Range(-1f, 1f),   // X: left/right
            Random.Range(0.7f, 1.2f),// Y: upward bias
            Random.Range(-1f, 1f)    // Z: forward/back
        ).normalized;

        rb.AddForce(randomDir * initial_force, ForceMode.Impulse);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale != 0.0f){
            //starting_speed = starting_speed + acceleration;
            Vector3 dir = (player.transform.position - transform.position).normalized;
            Vector3 desiredVelocity = dir * speed;
            Vector3 steering = desiredVelocity - rb.linearVelocity;
            rb.AddForce(steering, ForceMode.Acceleration);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
