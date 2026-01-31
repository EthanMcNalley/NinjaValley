using UnityEngine;

public class SmokeSpawn : MonoBehaviour
{
    public GameObject smoke_puff;

    void OnEnable()
    {
        Instantiate(smoke_puff, transform.position, Quaternion.identity);
    }
}
