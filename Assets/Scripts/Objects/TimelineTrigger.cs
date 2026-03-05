using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public GameObject timeline;
    Vector3 original_position;
    public Transform temp_position;
    public float timeline_time;
    float timer = 0;
    bool timeline_active = false;
    Collider player;
    public UnityEvent done_event;
    public GameObject TEMPBOSS;
    private PlayableDirector timeline_director;
    public string cinemachineTrack = "Cinemachine Track";
    private CinemachineBrain cinemachineBrain;
    
    void Awake()
    {
        timeline.SetActive(false);
        timeline_director = timeline.GetComponent<PlayableDirector>();
    }

    void Start()
    {
        TEMPBOSS = GameObject.FindGameObjectWithTag("Temp");
        cinemachineBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeline.SetActive(true);
            SetMainCamera();
            original_position = other.transform.position;
            other.transform.position = temp_position.position;
            other.GetComponent<NewMovement>().canMove = false;
            timeline_active = true;
            player = other;

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
        }
    }

    void Update()
    {
        if (timeline_active)
        {
            timer += Time.deltaTime;
            if (timer >= timeline_time)
            {
                //TEMPORARY //TEMPORARY //TEMPORARY
                TEMPBOSS.transform.GetChild(0).gameObject.SetActive(true);

                player.transform.position = original_position;
                player.GetComponent<NewMovement>().canMove = true;
                timeline_active = false;
                timeline.SetActive(false);
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
