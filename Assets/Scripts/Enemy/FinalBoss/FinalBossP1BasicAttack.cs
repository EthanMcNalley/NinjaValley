using System.Collections;
using UnityEngine;

public class FinalBossP1BasicAttack : MonoBehaviour
{
    public float followHeight = 19f;
    public float followSpeed = 10f;
    public float followDuration = 3.5f;

    public float dropDownWait = 1f;
    public float gravity = 40f;
    

    private float timer = 0f;

    private Transform player;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        if (player != null)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        while (timer <= followDuration)
        {
            //follow player
            Vector3 target = player.position + Vector3.up * followHeight;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * followSpeed);
            
            timer += Time.deltaTime;
            //wait for one frame which makes it the same as Update() :D
            yield return null;
        }
        
        //wait for drop
        yield return new WaitForSeconds(dropDownWait);
        
        //droping
        rb.isKinematic = false;
        rb.useGravity = true;
        
        rb.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
        
        Destroy(gameObject, 3f);
    }
    
    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
