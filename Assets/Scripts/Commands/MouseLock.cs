using UnityEngine;

public class MouseLock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
#endif
        
        if (UIManager.ui_state == UIManager.UIState.ACTIVE || NewMovement.is_dead){
            Cursor.lockState = CursorLockMode.None;
        }

        else{
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
