using System.Collections;
using UnityEngine;

public class ForceDisable : MonoBehaviour
{
    public GameObject gm;
    public bool changed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ForceDisableGM());
    }

    IEnumerator ForceDisableGM()
    {
        changed = true;
        yield return new WaitForSecondsRealtime(30.0f);
        gm.SetActive(false);
    }

    void Update()
    {
        if (changed) return;
        StartCoroutine(ForceDisableGM());
    }
}

