using UnityEngine;
using UnityEngine.Events;

public class DoEvent : MonoBehaviour
{
    public UnityEvent the_event;

    public void DoThisEvent()
    {
        the_event.Invoke();
    }
}
