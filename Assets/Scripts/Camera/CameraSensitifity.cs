using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


//YES I KNOW THERE IS A TYPO
public class CameraSensitifity : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController inputAxis;
    [SerializeField] private Slider xSlider;
    [SerializeField] private Slider ySlider;
    [SerializeField] private float controllerMuti = 1f;
    [SerializeField] private Toggle invertYToggle;
    private bool invertY;
    
    public static event System.Action<bool> OnControllerChanged;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xSlider.value = inputAxis.Controllers[0].Input.Gain;
        ySlider.value = inputAxis.Controllers[1].Input.Gain;
        xSlider.onValueChanged.AddListener(SetGainX);
        ySlider.onValueChanged.AddListener(SetGainY);
        invertY = invertYToggle.isOn;
        invertYToggle.onValueChanged.AddListener(SetInvertY);
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
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;
    
        // checking if input has changed
        bool hasActivity = false;
        foreach (var control in device.allControls)
        {
            if (!control.synthetic && !control.noisy && control.IsActuated(0.1f))
            {
                hasActivity = true;
                break;
            }
        }
    
        if (!hasActivity) return;

        switch (device)
        {
            case Mouse:
            case Keyboard:
                controllerMuti = 1f;
                OnControllerChanged?.Invoke(false);
                break;
            case Gamepad:
                controllerMuti = 60f;
                OnControllerChanged?.Invoke(true);
                break;
            default:
                controllerMuti = 1f;
                break;
        }
        SetGainX(xSlider.value);
        SetGainY(ySlider.value);
    }

    void SetGainX(float value)
    {
        inputAxis.Controllers[0].Input.Gain = value * controllerMuti;
    }
    
    void SetGainY(float value)
    {
        int invert = invertY ? 1 : -1;
        inputAxis.Controllers[1].Input.Gain = invert * value * controllerMuti;
    }
    
    void SetInvertY(bool value)
    {
        invertY = value;
        SetGainY(ySlider.value);
    }
}
