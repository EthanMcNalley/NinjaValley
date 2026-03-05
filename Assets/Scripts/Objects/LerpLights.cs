using UnityEngine;

public class LerpLights : MonoBehaviour
{
    public Light main_light;
    public Light change_light;
    public float speed = 1.0f;
    float current_time = 0;
    bool lerp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main_light = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp){   
            current_time = Time.deltaTime * speed;

            main_light.color = Color.Lerp(main_light.color, change_light.color, current_time);
            main_light.colorTemperature = Mathf.Lerp(main_light.colorTemperature, change_light.colorTemperature, current_time);
            main_light.intensity = Mathf.Lerp(main_light.intensity, change_light.intensity, current_time);
            main_light.transform.rotation = Quaternion.Lerp(main_light.transform.rotation,change_light.transform.rotation, current_time);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lerp = true;
        }
    }
}
