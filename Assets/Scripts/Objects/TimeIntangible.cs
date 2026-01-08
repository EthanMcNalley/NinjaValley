using UnityEngine;

public class TimeIntangible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.time_state == TimeManager.TimeState.NORMAL)
        {
            GetComponent<Collider>().enabled = false;
        }

        else
        {
            GetComponent<Collider>().enabled = true;
        }
    }
}
