using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
public class CameraMoveTrigger : MonoBehaviour
{
    public int new_priority = 10;
    public float increase_size = 5;
    public bool cutscene = false;
    public CinemachineCamera cinemachine_camera;
    CutsceneBars cutscene_bars;
    public float smoothing_amount = 1.0f;
    public bool different_exit_time = false;
    public float exit_time;
    Vector3 original_scale;
    void Start(){
        cutscene_bars = GameObject.FindGameObjectWithTag("UIManager").GetComponent<CutsceneBars>();
        original_scale = transform.localScale;
    }
    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Player")){
            if (cutscene){
                cutscene_bars.ActivateCutscene(cinemachine_camera.GetComponent<CutsceneCamera>().focusing_time);
                cinemachine_camera.GetComponent<CutsceneCamera>().FocusCameraSwitch(cinemachine_camera.GetComponent<CutsceneCamera>().focusing_time);
            }

            else{
                CameraControlling.smoothing_amount = smoothing_amount;
                cinemachine_camera.Priority = new_priority;
                transform.localScale = transform.localScale + (Vector3.one * increase_size);
            }
        }
    }

    void OnTriggerStay(Collider collider){

        if (!cutscene)
        {
            if (collider.CompareTag("Player")){
                CameraControlling.smoothing_amount = smoothing_amount;
                cinemachine_camera.Priority = new_priority;
                transform.localScale = original_scale + (Vector3.one * increase_size);
            }
        }
    }
    
    IEnumerator WaitToSwitch()
    {
        yield return new WaitForSeconds(0.1f);
        cinemachine_camera.Priority = new_priority;
    }
    void OnTriggerExit(Collider collider){
        if (collider.CompareTag("Player")){
            if (cutscene){
                Destroy(gameObject);
            }

            else{
                if (different_exit_time)
                {
                    CameraControlling.smoothing_amount = exit_time;
                }

                else
                {
                    CameraControlling.smoothing_amount = CameraControlling.original_smoothing_amount;
                }
                
                cinemachine_camera.Priority = 0;
                transform.localScale = transform.localScale - (Vector3.one * increase_size);
            }
        }
    }
}
