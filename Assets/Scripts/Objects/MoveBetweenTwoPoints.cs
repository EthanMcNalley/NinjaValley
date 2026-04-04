using Unity.VisualScripting;
using UnityEngine;

public class MoveBetweenTwoPoints : MonoBehaviour
{
    [SerializeField] bool forward = true;
    [SerializeField] Transform start_point;
    [SerializeField] Transform end_point;
    [SerializeField] float speed;
    [SerializeField] bool ignore_time_slow = false;
    float time = 0.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (forward){

            if (!ignore_time_slow)
            {
                time = time + Time.deltaTime * speed * TimeManager.slowed_amount;
            }

            else
            {
                time = time + Time.deltaTime * speed;
            }

            if (time > 1.0f){
                time = 1.0f;
                forward = false;
            }
        }

        else{
            if (!ignore_time_slow)
            {
                time = time - Time.deltaTime * speed * TimeManager.slowed_amount;
            }

            else
            {
                time = time - Time.deltaTime * speed;
            }

            if (time <= 0.0f){
                time = 0.0f;
                forward = true;
            }
        }

        float smoothed_time = Mathf.SmoothStep(0.0f, 1.0f, time);
        transform.position = Vector3.Slerp(start_point.position, end_point.position, smoothed_time);
    }
}
