using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    MovementInput movement_input;
    public Transform target_transform;
    public Transform camera_pivot;
    public Transform camera_transform;
    public LayerMask camera_collision_layers;
    public float default_position;
    private Vector3 camera_velocity = Vector3.zero;
    private Vector3 camera_vector_position;

    public float min_collision_offset = 0.2f;
    public float camera_collision_offset = 0.2f;
    public float camera_collision_radius = 0.2f;
    public float follow_speed = 0.2f;
    public float camera_look_speed = 2.0f;
    public float camera_pivot_speed = 2.0f;
    public float look_angle; //Vertical
    public float pivot_angle; //Horizontal/
    public float min_pivot_angle = -35.0f;
    public float max_pivot_angle = 35.0f;
    public bool follow_player = true;
    public Transform fixed_camera_position;

    private void Awake()
    {
        movement_input = FindAnyObjectByType<MovementInput>();
        target_transform = GameObject.FindGameObjectWithTag("Player").transform;
        camera_transform = Camera.main.transform;
        default_position = camera_transform.localPosition.z;
    }

    public void HandleAllCameraMovement(){
        if (follow_player){
            FollowTarget();
            RotateCamera();
            HandleCameraCollisions();
        }

        else{
            FixedPosition();
        }
    }

    private void FollowTarget(){
        Vector3 target_position = Vector3.SmoothDamp(transform.position, target_transform.position, ref camera_velocity, follow_speed);

        transform.position = target_position;
    }

    private void RotateCamera(){
        Vector3 rotation;
        Quaternion target_rotation;

        look_angle = look_angle + (movement_input.camera_horizontal_input * camera_look_speed);
        pivot_angle = pivot_angle - (movement_input.camera_vertical_input * camera_pivot_speed);
        pivot_angle = Mathf.Clamp(pivot_angle, min_pivot_angle, max_pivot_angle);

        rotation = Vector3.zero;
        rotation.y = look_angle;
        target_rotation = Quaternion.Euler(rotation);
        transform.rotation = target_rotation;

        rotation = Vector3.zero;
        rotation.x = pivot_angle;
        target_rotation = Quaternion.Euler(rotation);
        camera_pivot.localRotation = target_rotation;
    }

    private void HandleCameraCollisions(){
        float target_position = default_position;
        RaycastHit hit;

        Vector3 direction = camera_transform.position - camera_pivot.position;
        direction.Normalize();

        if (Physics.SphereCast(camera_pivot.transform.position, camera_collision_radius, direction, out hit, Mathf.Abs(target_position), camera_collision_layers)){
            float distance = Vector3.Distance(camera_pivot.position, hit.point);
            target_position =- (distance - camera_collision_offset);
        }

        if (Mathf.Abs(target_position) < min_collision_offset){
            target_position = target_position - min_collision_offset;
        }

        camera_vector_position.z = Mathf.Lerp(camera_transform.localPosition.z, target_position, 0.2f);
        camera_transform.localPosition = camera_vector_position;

    }

    private void FixedPosition(){
        camera_pivot.localRotation = fixed_camera_position.localRotation;
    }
    
    public void ChangeAngle(GameObject new_angle_object){
        follow_player = false;
        StartCoroutine(LerpToPosition(0.5f, new_angle_object.transform.position, false));
        transform.rotation = new Quaternion(new_angle_object.transform.rotation.x, new_angle_object.transform.rotation.y, new_angle_object.transform.rotation.z, new_angle_object.transform.rotation.w);
        // fixed_camera_position = new_angle_object.transform;

    }
    
    public void ResetCam()
    {
        follow_player = true;
      //StartCoroutine(LerpToPosition(0.05f, farLeft.position, false));    
    }

    IEnumerator LerpToPosition(float lerpSpeed, Vector3 newPosition, bool useRelativeSpeed = false)
    {    
        // if (useRelativeSpeed)
        // {
        //     float totalDistance = farRight.position.x - farLeft.position.x;
        //     float diff = transform.position.x - farLeft.position.x;
        //     float multiplier = diff / totalDistance;
        //     lerpSpeed *= multiplier;
        // }

        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            lerpSpeed = lerpSpeed + Time.deltaTime;
            Debug.Log(lerpSpeed);
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);

            transform.position = Vector3.Lerp(startingPos, newPosition, t);
            yield return 0;
        }    
    }
}
