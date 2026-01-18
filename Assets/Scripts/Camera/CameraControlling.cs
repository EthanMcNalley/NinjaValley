using Unity.Cinemachine;
using UnityEngine;

public class CameraControlling : MonoBehaviour
{
    public CinemachineOrbitalFollow cinemachine_orbital_follow;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FacePlayerBack();
    }

    public void FacePlayerBack(){
        cinemachine_orbital_follow.HorizontalAxis.Center = player.transform.rotation.eulerAngles.y;
    }
}
