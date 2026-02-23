using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public Transform teleport_pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleport_pos.position;
        }
    }
}
