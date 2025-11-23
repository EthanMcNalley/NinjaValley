using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour
{
    public float focusing_time;
    public void FocusCameraSwitch(float focus_time){
        focusing_time = focus_time;
        StartCoroutine(CameraMove());
    }

    IEnumerator CameraMove(){
        GetComponent<CinemachineCamera>().Priority = 100;

        yield return new WaitForSeconds(focusing_time);

        GetComponent<CinemachineCamera>().Priority = 0;
    }
}
