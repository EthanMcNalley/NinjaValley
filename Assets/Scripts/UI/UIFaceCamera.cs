using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Camera cam;
    public Transform target;
    public Vector3 offset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        target = this.transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
