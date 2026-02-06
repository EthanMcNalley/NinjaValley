using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Events;

public class CutsceneCamera : MonoBehaviour
{
    public UnityEvent cam_event;
    public float focusing_time;
    public void FocusCameraSwitch(float focus_time){
        focusing_time = focus_time;
        StartCoroutine(CameraMove());
    }

    IEnumerator CameraMove(){
        GetComponent<CinemachineCamera>().Priority = 100;

        if (cam_event != null)
        {
            cam_event.Invoke();
        }
        
        yield return new WaitForSeconds(focusing_time);

        GetComponent<CinemachineCamera>().Priority = 0;
    }
}
