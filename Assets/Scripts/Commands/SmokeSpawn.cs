using UnityEngine;

public class SmokeSpawn : MonoBehaviour
{
    // public float delay;
    public GameObject smoke_puff;

    void OnEnable()
    {
        SpawnWithSmoke();
    }

    public void SpawnWithSmoke()
    {
        GameObject smoke = Instantiate(smoke_puff, transform.position, Quaternion.Euler(-90, 0, 0), transform);
        smoke.transform.parent = null;
    }
}
