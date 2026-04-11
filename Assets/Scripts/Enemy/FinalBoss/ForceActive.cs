using System.Collections;
using UnityEngine;

public class ForceActive : MonoBehaviour
{
    public GameObject activeObject;
    public float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ForceActiveCoroutine());
    }

    IEnumerator ForceActiveCoroutine()
    {
        yield return new WaitForSeconds(time);
        activeObject.SetActive(true);
    }
}
