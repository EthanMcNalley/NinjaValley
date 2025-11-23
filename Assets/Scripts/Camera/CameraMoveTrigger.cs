using UnityEngine;
using Unity.Cinemachine;
public class CameraMoveTrigger : MonoBehaviour
{
    public CinemachineCamera cinemachine_camera;
    void Start(){
        cinemachine_camera = GetComponent<CinemachineCamera>();
    }
    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Player")){
            cinemachine_camera.Priority = 10;
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.CompareTag("Player")){
            cinemachine_camera.Priority = 0;
        }
    }
}
