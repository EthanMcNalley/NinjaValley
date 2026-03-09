using UnityEngine;

public class ToggleCameraForAnimationEvents : MonoBehaviour
{
    public void EnableCameraRotation()
    {
        CameraUtility.instance.EnableCameraRotation();
    }

    public void DisableCameraRotation()
    {
        CameraUtility.instance.DisableCameraRotation();
    }

    public void CameraRecenter()
    {
        CameraUtility.instance.InstantRecenter();
    }
}
