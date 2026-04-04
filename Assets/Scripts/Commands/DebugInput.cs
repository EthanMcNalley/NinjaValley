using UnityEngine;
using UnityEngine.Events;

public class DebugInput : MonoBehaviour
{
    public UnityEvent debug_event;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            debug_event.Invoke();
        }
    }
}
