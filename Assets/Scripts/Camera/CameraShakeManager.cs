using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance { get; private set; }
    
    private CinemachineImpulseDefinition impulseDefinition;
    
    public List<CinemachineImpulseListener> impulseListeners;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ScreenShakeFromProfile(ScreenShakeSO profile, CinemachineImpulseSource impulseSource)
    {
        //apply settings from profile
        SetupScreenShakeSettings(profile, impulseSource);
        
        //just shake :D
        impulseSource.GenerateImpulseWithForce(profile.impulseForce);
        Debug.Log("ScreenShake: " + profile.name);
    }

    private void SetupScreenShakeSettings(ScreenShakeSO profile, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.ImpulseDefinition;
        
        //impulse source stuff
        impulseSource.DefaultVelocity = profile.defaultVelocity;
        impulseDefinition.ImpulseDuration = profile.impulseTime;
        impulseDefinition.CustomImpulseShape = profile.impulseCurve;
        
        //impulse listener stuff
        foreach (var impulseListener in impulseListeners)
        {
            impulseListener.ReactionSettings.m_SecondaryNoise = profile.secondaryNoise;
            impulseListener.ReactionSettings.AmplitudeGain = profile.listenerAmplitude;
            impulseListener.ReactionSettings.FrequencyGain = profile.listenerFrequency;
            impulseListener.ReactionSettings.Duration = profile.listenerDuration;
        }
    }
}
