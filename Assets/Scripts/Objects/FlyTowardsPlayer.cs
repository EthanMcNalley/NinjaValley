using UnityEngine;

public class FlyTowardsPlayer : MonoBehaviour
{
    GameObject player;
    public float starting_speed = 1.0f;
    public float acceleration = 0.01f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale != 0.0f){
            acceleration = acceleration + Time.fixedDeltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, starting_speed * acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
