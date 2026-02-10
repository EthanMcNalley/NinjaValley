using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AllObjectsGoneEvent : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public UnityEvent done_event;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == null)
            {
                objects.RemoveAt(i);
            }
        }

        if(objects.Count == 0)
        {
            done_event.Invoke();
        }
    }
}
