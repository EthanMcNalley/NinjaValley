using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraUtility : MonoBehaviour
{
    public static CameraUtility instance { get; private set; }
    [SerializeField] private CinemachineOrbitalFollow orbitalFollow;
    [SerializeField] private CinemachineInputAxisController inputAxis;
    //[SerializeField] private const float horizontalCenter = 0;
    [SerializeField]private const float verticalCenter = 14f;
    [SerializeField] private Transform followTarget;
    [SerializeField] private InputActionReference recenter; //Null for now because it's working weird

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantRecenter();
    }

    void OnEnable()
    {
        if (recenter != null)
        {
            recenter.action.performed += OnRecenterPerformed;
        }
    }   
    
    void OnDisable()
    {
        if (recenter != null)
        {
            recenter.action.performed -= OnRecenterPerformed;
        }
    }

    void OnRecenterPerformed(InputAction.CallbackContext ctx)
    {
        InstantRecenter();
    }
    
    public void InstantRecenter()
    {
        if (orbitalFollow == null || followTarget == null) return;
        orbitalFollow.HorizontalAxis.Value = followTarget.eulerAngles.y;
        orbitalFollow.VerticalAxis.Value = verticalCenter;
    }

    public void DisableCameraRotation()
    {
        if (inputAxis == null) return;
        foreach (var controller in inputAxis.Controllers)
        {
            controller.Enabled = false;
        }
    }
    
    public void EnableCameraRotation()
    {
        if (inputAxis == null) return;
        foreach (var controller in inputAxis.Controllers)
        {
            controller.Enabled = true;
        }
    }
}
