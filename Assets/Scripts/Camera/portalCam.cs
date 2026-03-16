using UnityEngine;

public class portalCam : MonoBehaviour
{
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;
    
    public Camera portalCamera;
    public Renderer portalRenderer;
    public RenderTexture renderTexture;

    void Start()
    {
        playerCamera = Camera.main.transform;
        portalCamera = GetComponent<Camera>();
        portalRenderer = portal.GetComponent<Renderer>();
    }
    
    // Update is called once per frame
    void Update()
    {/*
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;
        
        float angularDiff = Quaternion.Angle(portal.rotation, otherPortal.rotation);
        
        Quaternion portalRotationDiff =  Quaternion.AngleAxis(angularDiff, Vector3.up);
        Vector3 newCameraDirection = portalRotationDiff * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);*/
        
        Quaternion flip = Quaternion.AngleAxis(180f, portal.up);
        Quaternion rotationDiff = flip * portal.rotation * Quaternion.Inverse(otherPortal.rotation);
        
        transform.rotation = rotationDiff * playerCamera.rotation;
    }
}
