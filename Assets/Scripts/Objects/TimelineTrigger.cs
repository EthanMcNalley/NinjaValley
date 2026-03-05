using UnityEngine;

public class TimelineTrigger : MonoBehaviour
{
    public GameObject timeline;
    void Awake()
    {
        timeline.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeline.SetActive(true);
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
