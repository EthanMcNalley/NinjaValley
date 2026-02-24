using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.Events;

public class CutsceneCamera : MonoBehaviour
{
    public UnityEvent cam_event;
    public float focusing_time;
    public bool recenter = false;
    
    public void FocusCameraSwitch(float focus_time){
        focusing_time = focus_time;
        StartCoroutine(CameraMove());
    }

    IEnumerator CameraMove(){
        GetComponent<CinemachineCamera>().Priority = 100;

        if (recenter)
        {
            CameraEvents.RaiseSaveCameraAxisValue();
        }

        if (cam_event != null)
        {
            cam_event.Invoke();
        }
        
        yield return new WaitForSeconds(focusing_time);

        if (recenter)
        {
            CameraEvents.RaiseCameraRecenter();
        }
        
        GetComponent<CinemachineCamera>().Priority = 0;
    }

    public void EnableAreaText()
    {
        AreaText.active = true;
    }
}
