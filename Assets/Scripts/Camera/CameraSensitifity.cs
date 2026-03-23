using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem.LowLevel;


//YES I KNOW THERE IS A TYPO
public class CameraSensitifity : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController inputAxis;
    [SerializeField] private Slider xSlider;
    [SerializeField] private Slider ySlider;
    [SerializeField] private float controllerMuti = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xSlider.value = inputAxis.Controllers[0].Input.Gain;
        ySlider.value = inputAxis.Controllers[1].Input.Gain;
        xSlider.onValueChanged.AddListener(SetGainX);
        ySlider.onValueChanged.AddListener(SetGainY);
    }

    void OnEnable()
    {
        InputSystem.onEvent += (eventPtr, device) => OnDeviceChange(eventPtr, device);
    }

    void OnDisable()
    {
        InputSystem.onEvent -= (eventPtr, device) => OnDeviceChange(eventPtr, device);
    }

    void OnDeviceChange(InputEventPtr eventPtr, InputDevice device)
    {
        switch (device)
        {
            case Mouse:
                controllerMuti = 1f;
                SetGainX(xSlider.value);
                SetGainY(ySlider.value);
                break;
            case Gamepad:
                controllerMuti = 60f;
                SetGainX(xSlider.value);
                SetGainY(ySlider.value);
                break;
            default:
                controllerMuti = 1f;
                SetGainX(xSlider.value);
                SetGainY(ySlider.value);
                break;
        }
    }

    void SetGainX(float value)
    {
        inputAxis.Controllers[0].Input.Gain = value * controllerMuti;
    }
    
    void SetGainY(float value)
    {
        inputAxis.Controllers[1].Input.Gain = value * controllerMuti;
    }
}
