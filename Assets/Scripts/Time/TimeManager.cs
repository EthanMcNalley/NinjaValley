using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    PlayerInput player_controls;
    
    public static TimeState time_state;
    public float time_timer = 0.0f;
    public float time_slowed_down = 3.0f;
    public static float slowed_amount = 0.1f;
    public float slow_amount = 0.1f;
    public GameObject volume;
    
    private InputAction timeSlowAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time_timer = time_slowed_down;
        time_state = TimeState.NORMAL;
        
        if (InputSystem.actions)
        {
            timeSlowAction = InputSystem.actions.FindAction("TimeSlow");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if(volume.profile)){
        //     original_saturation_value = adjustments.saturation.value;
        // }
        //Debug.Log(Time.fixedDeltaTime);
        if (time_state == TimeState.NORMAL){
            volume.SetActive(false);
            if (timeSlowAction.triggered){
                time_state = TimeState.SLOWED;
                time_timer = 0.0f;
                //Time.fixedDeltaTime = 0.02f * slowed_amount;
            }
        }

        else{
            volume.SetActive(true);
            // Time.timeScale = slowed_amount;
            time_timer = time_timer + Time.deltaTime;
            Debug.Log(time_timer);

            if (timeSlowAction.triggered){
                time_timer = time_slowed_down;
            }

            if (time_timer >= time_slowed_down){
                time_state = TimeState.NORMAL;
                //Time.fixedDeltaTime = 0.02f;
                //Time.timeScale = 1.0f;
                
            }
        }

        if (time_state == TimeState.NORMAL){
            slowed_amount = 1.0f;
        }

        else{
            slowed_amount = slow_amount;
        }
    }

    public enum TimeState{
        NORMAL,
        SLOWED
    }

    public void SlowTime(){

    }

    public void NormalTime(){

    }
}
