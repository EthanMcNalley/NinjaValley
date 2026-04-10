using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class TimelineTrigger : MonoBehaviour
{
    public GameObject timeline;
    public Transform original_position;
    public Transform temp_position;
    public float timeline_time;
    float timer = 0;
    bool timeline_active = false;
    Collider player;
    public UnityEvent done_event;
    public GameObject boss;
    private PlayableDirector timeline_director;
    public string cinemachineTrack = "Cinemachine Track";
    private CinemachineBrain cinemachineBrain;
    private bool first_time = true;
    
    void Awake()
    {
        timeline.SetActive(false);
        timeline_director = timeline.GetComponent<PlayableDirector>();
    }

    void Start()
    {
        cinemachineBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (first_time){
                timeline.SetActive(true);
                SetMainCamera();
                //original_position = other.transform.position;
                other.transform.position = temp_position.position;
                other.GetComponent<NewMovement>().canMove = false;
                timeline_active = true;
                player = other;
                first_time = false;
            }

            else
            {
                gameObject.GetComponent<Collider>().enabled = false;
                boss.SetActive(true);
                Debug.Log("SUFFERING");
            }
        }
    }

    void SetMainCamera()
    {
        PlayableAsset playableAsset = timeline_director.playableAsset;
        if (playableAsset != null)
        {
            foreach (var track in playableAsset.outputs)
            {
                if (track.streamName == cinemachineTrack)
                {
                    timeline_director.SetGenericBinding(track.sourceObject, cinemachineBrain);
                    break;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    void Update()
    {
        if (timeline_active)
        {
            timer += Time.deltaTime;
            if (timer >= timeline_time)
            {
                player.transform.position = original_position.position;
                //TEMPORARY //TEMPORARY //TEMPORARY
                //TEMPBOSS.transform.GetChild(0).gameObject.SetActive(true);
                boss.SetActive(true);
                Debug.Log("workds");
                player.GetComponent<NewMovement>().canMove = true;
                timeline_active = false;
                timeline.SetActive(false);
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        else
        {
            if (NewMovement.is_dead)
            {
                StartCoroutine(EnableNow());
            }
        }
    }

    IEnumerator EnableNow()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.GetComponent<Collider>().enabled = true;

    }
}
