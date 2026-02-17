using Unity.Cinemachine;
using UnityEngine;

public class CameraRecenterListener : MonoBehaviour
{
    public CinemachineOrbitalFollow cinemachineOrbitalFollow;
    public float horizontalAxisValue, verticalAxisValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachineOrbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    void OnEnable()
    {
        CameraEvents.CameraRecenter += CameraEventsOnCameraRecenter;
        CameraEvents.SaveCameraAxisValue += CameraEventsOnSaveCameraAxisValue;
    }

    void OnDisable()
    {
        CameraEvents.CameraRecenter -= CameraEventsOnCameraRecenter;
        CameraEvents.SaveCameraAxisValue -= CameraEventsOnSaveCameraAxisValue;
    }

    void OnDestroy()
    {
        CameraEvents.CameraRecenter -= CameraEventsOnCameraRecenter;
        CameraEvents.SaveCameraAxisValue -= CameraEventsOnSaveCameraAxisValue;
    }

    private void CameraEventsOnSaveCameraAxisValue()
    {
        horizontalAxisValue = cinemachineOrbitalFollow.HorizontalAxis.Value;
        verticalAxisValue = cinemachineOrbitalFollow.VerticalAxis.Value;
    }

    private void CameraEventsOnCameraRecenter()
    {
        cinemachineOrbitalFollow.HorizontalAxis.Value = horizontalAxisValue;
        cinemachineOrbitalFollow.VerticalAxis.Value = verticalAxisValue;
        Debug.Log("Camera Recentered");
    }
}
