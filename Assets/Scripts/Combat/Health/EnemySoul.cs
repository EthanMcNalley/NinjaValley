using UnityEngine;

public class EnemySoul : MonoBehaviour
{
    private Camera cam;
    public Transform target;
    public Vector3 offset;
    
    public Animator animator;
    private bool markedForDeath = false;
    private bool markedForExecute = false;
    private bool shadow = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        
        markedForDeath = false;
        markedForExecute = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
    
    void UpdateSoulUI()
    {
        animator.SetBool("MarkedForDeath", markedForDeath);
        animator.SetBool("MarkedForExecute", markedForExecute);
        animator.SetBool("ShadowEnter", shadow);
    }

    public void SetShadow(bool value)
    {
        shadow = value;
        UpdateSoulUI();
    }
    
    public void SetMarkedForDeath(bool value)
    {
        markedForDeath = value;
        UpdateSoulUI();
    } 
    
    public void SetMarkedForExecute(bool value)
    {
        markedForExecute = value;
        markedForDeath = false;
        UpdateSoulUI();
    }
}
