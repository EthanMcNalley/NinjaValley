using UnityEngine;
using System.Collections;

public class SpawnHereAtStart : MonoBehaviour
{
    Transform spawn_point;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return null;
        
        spawn_point = GetComponent<Transform>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawn_point.position;
    }
}
