using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScreenShake/New ScreenShake SO")]
public class ScreenShakeSO : ScriptableObject
{
    [Header("Impluse Source Settings")]
    public float impulseTime = 1f;
    public float impulseForce = 1f;
    public Vector3 defaultVelocity = new  Vector3(0f, -1f, 0f);
    public AnimationCurve impulseCurve;
    
    [Header("Impulse Listener Settings")]
    public float listenerAmplitude = 1f;
    public float listenerFrequency = 1f;
    public float listenerDuration = 1f;
}
