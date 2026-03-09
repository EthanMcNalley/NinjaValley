using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;


//YES I KNOW THERE IS A TYPO
public class CameraSensitifity : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController inputAxis;
    [SerializeField] private Slider xSlider;
    [SerializeField] private Slider ySlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xSlider.value = inputAxis.Controllers[0].Input.Gain;
        ySlider.value = inputAxis.Controllers[1].Input.Gain;
        xSlider.onValueChanged.AddListener(SetGainX);
        ySlider.onValueChanged.AddListener(SetGainY);
    }

    void SetGainX(float value)
    {
        inputAxis.Controllers[0].Input.Gain = value;
    }
    
    void SetGainY(float value)
    {
        inputAxis.Controllers[1].Input.Gain = value;
    }
}
