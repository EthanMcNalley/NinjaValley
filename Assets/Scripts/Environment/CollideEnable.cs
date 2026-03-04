using UnityEngine;
using System.Collections;

public class CollideEnable : MonoBehaviour
{
    public GameObject[] gameObjects;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObjects.Length > 0)
        {
            foreach (GameObject g in gameObjects)
            {
                g.SetActive(true);
            }
        }
    }
}
