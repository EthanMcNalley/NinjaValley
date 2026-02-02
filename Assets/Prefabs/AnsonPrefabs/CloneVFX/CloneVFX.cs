using System;
using UnityEngine;

public class CloneVFX : MonoBehaviour
{
    //private Animator animator;
    private Rigidbody rb;
    private float countDown = 0.3f;
    public float duration = 0.3f;
    public float speed;
    public GameObject player;
    public GameObject endPosition;
    public GameObject startPosition;
    [SerializeField]private bool endPositionActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        countDown = duration;
    }
    
    private void OnEnable()
    {
        
        transform.root.position = player.transform.position;
        transform.root.rotation = player.transform.rotation;
        transform.position = startPosition.transform.position;
        transform.rotation = startPosition.transform.rotation;
    }

    private void OnDisable()
    {
        countDown = duration;
        endPositionActive = false;
        rb.linearVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (countDown <= (duration / 1.8f) && !endPositionActive)
        {
            rb.linearVelocity = transform.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == endPosition)
        {
            endPositionActive = true;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
