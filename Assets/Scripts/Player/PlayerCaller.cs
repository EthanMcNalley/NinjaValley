using UnityEditor;
using UnityEngine;

public class PlayerCaller : MonoBehaviour
{
    MovementInput movement_input;
    GameObject cam;
    PlayerMovement player_movement;
    public UIManager ui_manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        movement_input = GetComponent<MovementInput>();
        cam =  GameObject.FindGameObjectWithTag("MainCamera");
        player_movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        movement_input.HandleInputs();
    
            //UIManager.activate = true;
            // if (UIManager.ui_state == UIManager.UIState.INACTIVE){
            //     UIManager.activate = true;
            // }

            // else{

            // }

    }

    private void FixedUpdate()
    {
        player_movement.HandleAllMovement();
    }

    private void LateUpdate()
    {
        //camera_manager.HandleAllCameraMovement( );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraChanger")){
            //camera_manager.ChangeAngle(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CameraChanger")){
            //camera_manager.ResetCam();
        }
    }
}
