using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] bool special_teleport = false;
    public Transform teleport_pos;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!special_teleport)
            {
                other.transform.position = teleport_pos.position;
            }

            else
            {
                if (other.GetComponent<NewMovement>().player_renderer.enabled)
                {
                    StartCoroutine(other.GetComponent<NewMovement>().PoofUnpoof(other.GetComponent<NewMovement>(), teleport_pos.position));
                }
            }
        }
    }
}
