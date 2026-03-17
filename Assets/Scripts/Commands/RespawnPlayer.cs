using System.Collections;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = NewMovement.last_grounded_position;
            // StartCoroutine(Respawning(other));
        }
    }

    IEnumerator Respawning(Collider player)
    {
        player.GetComponent<NewMovement>().DisableMovement();
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<NewMovement>().player_renderer.enabled = false;
        // player.transform.position = NewMovement.last_grounded_position;
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<NewMovement>().player_renderer.enabled = false;
    }
}
