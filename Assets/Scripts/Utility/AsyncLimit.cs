using UnityEngine;

public class AsyncLimit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }
}
