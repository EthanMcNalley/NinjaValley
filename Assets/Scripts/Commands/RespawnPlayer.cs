using System.Collections;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public bool revive = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!revive)
            {
                other.transform.position = NewMovement.last_grounded_position;
            }

            else
            {
                other.transform.position = NewMovement.revive_position;
                gameObject.SetActive(false);
            }
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
