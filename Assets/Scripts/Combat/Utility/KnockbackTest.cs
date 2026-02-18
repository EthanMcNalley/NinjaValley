using UnityEngine;

public class KnockbackTest : MonoBehaviour
{
    public NewMovement playerMovement;
    public float force = 2f, height = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement =  GameObject.FindGameObjectWithTag("Player").GetComponent<NewMovement>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = other.transform.position - transform.position;
            playerMovement.KnockbackPlayer(direction, force, height);
        }
    }
}
