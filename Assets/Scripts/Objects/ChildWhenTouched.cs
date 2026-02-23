using UnityEngine;

public class ChildWhenTouched : MonoBehaviour
{
    public GameObject player;
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("ugh");
    //         other.transform.parent = transform;
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.transform.parent = null;
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChildCheck"))
        {
            Debug.Log("wowjustwow");
            player.transform.parent = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("HUHHHH");
        if (other.CompareTag("ChildCheck"))
        {
            Debug.Log("AND??");
            player.transform.parent = null;
        }
    }
}
