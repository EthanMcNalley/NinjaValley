using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockIn : MonoBehaviour
{
    [SerializeField] private GameObject lockOnCamera;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private LayerMask enemyLayerMask; 
    [SerializeField] private LayerMask interactableLayerMask;
    public GameObject target;
    public float lockOnDistance = 50f;
    private InputAction lockOnAction;
    private bool lockOn;
    private GameObject lockOnIcon;

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
            LockOnFind();
        }
        else
        {
            ClearLockOn();
            return;
        }

        if (lockOn && target != null)
        {
            LockOn();
        }
    }

    void LockOnFind()
    {
        target = FindClosest.FindClosestGameObject(this.transform.position, lockOnDistance, enemyLayerMask);
        if (target != null)
        {
            return;
        }
        
        target = FindClosest.FindClosestGameObject(this.transform.position, lockOnDistance, interactableLayerMask);
        if (target != null)
        {
            return;
        }
        
        Debug.Log("No target found");
        lockOn = false;
    }

    void LockOn()
    {
        lockOnIcon = target.transform.Find("LockOnIcon").gameObject;
        
        if (lockOnIcon != null)
        {
            lockOnIcon.SetActive(true);
        }
        
        cinemachineCamera.LookAt = target.transform; 
        lockOnCamera.SetActive(true);
    }

    public void ClearLockOn()
    {
        lockOnCamera.SetActive(false);
        if (lockOnIcon != null)
        {
            lockOnIcon.gameObject.SetActive(false);
            lockOnIcon = null;
        }
        lockOn = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!lockOn) return;

        if (target == null)
        {
            ClearLockOn();
            return;
        }
        
        if (!target.activeInHierarchy)
        {
            ClearLockOn();
        }
    }

    public bool IsLockOn()
    {
        return lockOn;
    }
}
