using System.Collections;
using UnityEngine;

public class GroupActivate : MonoBehaviour
{
    public GameObject[] objects;
    public float[] delay_times;
    int temp_object;
    public void ActivateWithDelay(int object_index)
    {
        temp_object = object_index;
        Invoke("InvokeActivate", delay_times[object_index]);
    }

    void InvokeActivate()
    {
        objects[temp_object].SetActive(true);
    }
}
