using UnityEngine;
using UnityEngine.Events;

public class DoEvent : MonoBehaviour
{
    public UnityEvent the_event;

    public void DoThisEvent()
    {
        if (the_event != null){
            the_event.Invoke();
        }
    }
}
