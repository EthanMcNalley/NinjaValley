using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = NewMovement.last_grounded_position;
        }
    }
}
