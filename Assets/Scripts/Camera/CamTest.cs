using Unity.Cinemachine;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash)){
            cinemachineCamera.Priority = 100;
        }

        if (Input.GetKeyDown(KeyCode.Slash)){
            cinemachineCamera.Priority = 0;
        }
    }
}
