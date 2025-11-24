using UnityEngine;
using Unity.Cinemachine;
public class CameraMoveTrigger : MonoBehaviour
{
    public float increase_size = 5;
    public bool cutscene = false;
    public CinemachineCamera cinemachine_camera;
    CutsceneBars cutscene_bars;
    void Start(){
        cutscene_bars = GameObject.FindGameObjectWithTag("UIManager").GetComponent<CutsceneBars>();
    }
    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Player")){
            if (cutscene){
                cutscene_bars.ActivateCutscene(cinemachine_camera.GetComponent<CutsceneCamera>().focusing_time);
                cinemachine_camera.GetComponent<CutsceneCamera>().FocusCameraSwitch(cinemachine_camera.GetComponent<CutsceneCamera>().focusing_time);
            }

            else{
                cinemachine_camera.Priority = 10;
                transform.localScale = transform.localScale + (Vector3.one * increase_size);
            }
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.CompareTag("Player")){
            if (cutscene){
                Destroy(gameObject);
            }

            else{
                cinemachine_camera.Priority = 0;
                transform.localScale = transform.localScale - (Vector3.one * increase_size);
            }
        }
    }
}
