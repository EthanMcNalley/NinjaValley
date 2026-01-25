using Unity.Cinemachine;
using UnityEngine;

public class CameraControlling : MonoBehaviour
{
    public CinemachineBrain main_cam_brain;
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
    // public static float original_smoothing_amount;
    // public static float smoothing_amount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // original_smoothing_amount = main_cam_brain.DefaultBlend.BlendTime;
        // smoothing_amount = main_cam_brain.DefaultBlend.BlendTime;
    }

    // Update is called once per frame
    void Update()
    {
        //main_cam_brain.DefaultBlend.BlendCurve = 
        FacePlayerBack();
    }

    public void FacePlayerBack(){
        cinemachine_orbital_follow.HorizontalAxis.Center = player.transform.rotation.eulerAngles.y;
    }
}
