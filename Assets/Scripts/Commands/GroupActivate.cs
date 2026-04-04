using System.Collections;
using UnityEngine;

public class GroupActivate : MonoBehaviour
{
    public GameObject[] objects;
    public float[] delay_times;
    public void ActivateWithDelay()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            StartCoroutine(Activate(objects[i], delay_times[i]));
        }
    }

    IEnumerator Activate(GameObject game_object, float delay)
    {
        yield return new WaitForSeconds(delay);
        game_object.SetActive(true);
    }
}
