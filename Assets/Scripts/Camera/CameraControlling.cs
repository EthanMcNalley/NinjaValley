using Unity.Cinemachine;
using UnityEngine;

public class CameraControlling : MonoBehaviour
{
    public CinemachineCamera cinemachine_camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachine_camera = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeTarget(Transform new_target){
        cinemachine_camera.Follow = new_target.transform;
    }
}
