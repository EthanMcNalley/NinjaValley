using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float life_time = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, life_time);
    }
}
