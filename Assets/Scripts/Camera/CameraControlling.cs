using Unity.Cinemachine;
using UnityEngine;

public class CameraControlling : MonoBehaviour
{
    public CinemachineOrbitalFollow cinemachine_orbital_follow;
    public GameObject player;
    private float current_player_roto;
    public float player_roto{
        get { return current_player_roto; }

        set
        {
            if (current_player_roto != value)
            {
                current_player_roto = value;
                FacePlayerBack();
            }
        }
    }
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
