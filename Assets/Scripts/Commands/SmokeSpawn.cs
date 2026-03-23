using UnityEngine;
using FMODUnity;

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
        // AudioManager.instance.PlayOneShot("event:/Environment/Poof", transform.position);
        smoke.transform.parent = null;
    }
}
