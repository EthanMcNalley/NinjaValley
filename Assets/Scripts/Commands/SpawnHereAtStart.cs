using UnityEngine;

public class SpawnHereAtStart : MonoBehaviour
{
    Transform spawn_point;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawn_point = GetComponent<Transform>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawn_point.position;
    }
}
