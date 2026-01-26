using UnityEngine;

public class CopyTraits : MonoBehaviour
{
    Camera this_cam;
    Camera main_cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this_cam = GetComponent<Camera>();
        main_cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this_cam.fieldOfView = main_cam.fieldOfView;
    }
}
