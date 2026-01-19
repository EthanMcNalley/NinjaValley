using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockIn : MonoBehaviour
{
    [SerializeField] private GameObject lockOnCamera;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private LayerMask layerMask;
    public float lockOnDistance = 50f;
    private InputAction lockOnAction;
    private bool lockOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()
    {
        lockOnCamera.SetActive(false);
        cinemachineCamera = lockOnCamera.GetComponent<CinemachineCamera>();
    }*/

    void Awake()
    {
        lockOnCamera.SetActive(false);
        cinemachineCamera = lockOnCamera.GetComponent<CinemachineCamera>();
        if (InputSystem.actions)
        {
            lockOnAction = InputSystem.actions.FindAction("LockIn");
        }
    }

    void OnEnable()
    {
        lockOnAction.Enable();
        lockOnAction.performed += OnLockOnActionPerformed;
    }

    void OnDisable()
    {
        lockOnAction.Disable();
        lockOnAction.performed -= OnLockOnActionPerformed;
    }
    
    void OnLockOnActionPerformed(InputAction.CallbackContext ctx)
    {
        lockOn = !lockOn;
        
        if (lockOn)
        {
            LockOn();
        }
        else
        {
            lockOnCamera.SetActive(false);
        }
    }

    void LockOn()
    {
        GameObject target = FindClosest.FindClosestGameObject(this.transform.position, lockOnDistance, layerMask);
        if (target == null)
        {
            Debug.Log("No target found");
            lockOn = false;
            return;
        }
        
        cinemachineCamera.LookAt =  target.transform; 
        lockOnCamera.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DisableLockOn()
    {
        lockOn = false;
        lockOnCamera.SetActive(false);
    }
}
