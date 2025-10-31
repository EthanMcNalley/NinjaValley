using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeState time_state;
    public float time_timer = 0.0f;
    public float time_slowed_down = 3.0f;
    public static float slowed_amount = 0.1f;
    public float slow_amount = 0.1f;
    public float refresh_time = 3.0f;
    public float refresh_timer = 0.0f;
    public GameObject volume;
    private GameObject[] anim_objects;
    
    private InputAction timeSlowAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time_timer = time_slowed_down;
        time_state = TimeState.NORMAL;
        refresh_timer = refresh_time;

        if (InputSystem.actions)
        {
            timeSlowAction = InputSystem.actions.FindAction("TimeSlow");
        }

        anim_objects = GameObject.FindGameObjectsWithTag("Test");
    }

    // Update is called once per frame
    void Update()
    {
        // if(volume.profile)){
        //     original_saturation_value = adjustments.saturation.value;
        // }

            if (time_state == TimeState.NORMAL){
                volume.SetActive(false);

                if (refresh_timer >= refresh_time){
                    if (timeSlowAction.triggered){
                        time_timer = 0.0f;
                        refresh_timer = 0.0f;
                        time_state = TimeState.SLOWED;
                        //Time.fixedDeltaTime = 0.02f * slowed_amount;
                    }
                }
            }

            else{
                volume.SetActive(true);
                // Time.timeScale = slowed_amount;
                //Debug.Log(time_timer);

                if (time_timer >= time_slowed_down){
                    time_state = TimeState.NORMAL;
                    //Time.fixedDeltaTime = 0.02f;
                    //Time.timeScale = 1.0f;
                    
                }

                if (timeSlowAction.triggered){
                    refresh_timer = time_slowed_down - time_timer;
                    time_timer = time_slowed_down;
                    time_state = TimeState.NORMAL;
                }
            }

        if (time_timer < time_slowed_down){
            time_timer = time_timer + Time.deltaTime;
        }

        else{
            if (refresh_timer < refresh_time){
                refresh_timer = refresh_timer + Time.deltaTime;
            }
        }

        if (time_state == TimeState.NORMAL){
            slowed_amount = 1.0f;
        }

        else{
            slowed_amount = slow_amount;
        }

        for (int i = 0; i < anim_objects.Length; i++){
            anim_objects[i].GetComponent<Animator>().SetFloat("Speed",  slowed_amount);
        }
    }

    public enum TimeState{
        NORMAL,
        SLOWED
    }

    public TimeState GetTimeState()
    {
        return time_state;
    }

    public void SlowTime(){

    }

    public void NormalTime(){

    }
}
