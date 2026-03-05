using UnityEngine;
using UnityEngine.Events;

public class DestroyEvent : MonoBehaviour
{
    public UnityEvent destroy_event;

    void OnDestroy()
    {
        destroy_event.Invoke();
    }
}
