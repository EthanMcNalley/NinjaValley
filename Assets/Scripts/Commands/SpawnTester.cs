using UnityEngine;
using UnityEngine.AI;

public class SpawnTester : MonoBehaviour
{
    public GameObject object_to_spawn;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(object_to_spawn, transform.position, Quaternion.identity);
        }
    }
}
