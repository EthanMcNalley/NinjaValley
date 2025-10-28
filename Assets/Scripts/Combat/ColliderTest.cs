using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
    }
}
