using System;
using UnityEngine;

public static class CameraEvents
{
    public static event Action CameraRecenter;
    public static event Action SaveCameraAxisValue;
    
    public static void RaiseCameraRecenter() => CameraRecenter?.Invoke();
    public static void RaiseSaveCameraAxisValue() => SaveCameraAxisValue?.Invoke();
}
