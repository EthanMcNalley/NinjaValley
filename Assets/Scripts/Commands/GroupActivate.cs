using System.Collections;
using UnityEngine;

public class GroupActivate : MonoBehaviour
{
    public GameObject[] objects;
    public float[] delay_times;
    int temp_object;
    public void ActivateWithDelay()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            temp_object = i;
            Invoke("InvokeActivate", delay_times[temp_object]);
        }
    }

    void InvokeActivate()
    {
        objects[temp_object].SetActive(true);
    }
}
