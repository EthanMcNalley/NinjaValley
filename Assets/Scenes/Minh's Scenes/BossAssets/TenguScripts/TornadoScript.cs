using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.up * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
    }
}
