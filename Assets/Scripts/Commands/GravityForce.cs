using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityForce : MonoBehaviour
{
    public float gravity_scale = 1.0f;
    float slowed_gravity;
    float normal_gravity;
    public static float global_gravity_scale = -9.81f;
    private Rigidbody rb;
    private float prior_velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //IF START DOESNT WORK TRY USING ONENABLE INSTEAD 
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Debug.Log("WOOOO");
        slowed_gravity = gravity_scale * TimeManager.slowed_amount;
        normal_gravity = gravity_scale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 new_gravity = global_gravity_scale * gravity_scale * Vector3.up;


        if (TimeManager.time_state == TimeManager.TimeState.NORMAL || rb.gameObject.CompareTag("Player")){
            rb.AddForce(new_gravity, ForceMode.Acceleration);
            prior_velocity = rb.linearVelocity.y;
        }

        else{
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, prior_velocity / 20.0f, rb.linearVelocity.z);
        }
    }

    void Update()
    {
        if (!rb.gameObject.CompareTag("Player")){
            if (TimeManager.time_state == TimeManager.TimeState.SLOWED){
                gravity_scale = slowed_gravity;
            }

            else{
                gravity_scale = normal_gravity;
            }
        }
    }
}
