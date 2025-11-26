using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class LinkedSwitch : MonoBehaviour
{
    public UnityEvent switch_action;
    public bool done = false;
    public Material done_material;
    public Light done_light;
    public Color done_light_color;
    CutsceneBars cutscene_bars;
    public GameObject hit_particle;
    public EventReference switch_hit_sfx;
    public LinkedSwitch[] linked_switches;
    public bool turned_on;
    public Material on_material;
    public Material off_material;
    public float time_on = 0.5f;
    private float time_on_timer = 0.0f;
    void Start(){
        cutscene_bars = GameObject.FindGameObjectWithTag("UIManager").GetComponent<CutsceneBars>();
        time_on_timer = time_on;
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("AttackHitBox"))
        {
            if (!done){
                AudioManager.instance.PlayOneShot(switch_hit_sfx, transform.position);
                turned_on = true;

                for (int i = 0; i < linked_switches.Length; i++){
                    if (!linked_switches[i].turned_on){
                        time_on_timer = 0.0f;
                        return;
                    }
                }

                switch_action.Invoke();
                done = true;
            }

            if (done){
                for (int i = 0; i < linked_switches.Length; i++){
                    linked_switches[i].done = true;
                    linked_switches[i].GetComponent<MeshRenderer>().material = done_material;
                    linked_switches[i].done_light.color = done_light_color;
                }
            }

            // Rigidbody sword_rb = collider.GetComponent<Rigidbody>();
            // Vector3 particle_direction = sword_rb.linearVelocity.normalized;
            // Quaternion correct_roto = Quaternion.LookRotation(-particle_direction);
            Quaternion correct_roto = Quaternion.LookRotation(-(collider.ClosestPoint(transform.position) - collider.transform.position).normalized);
            Instantiate(hit_particle, collider.ClosestPoint(transform.position), correct_roto);
        }
    }

    void Update(){
        if (!done){
            if (time_on_timer < time_on){
                turned_on = true;
                time_on_timer += (Time.deltaTime * TimeManager.slowed_amount);
                GetComponent<MeshRenderer>().material = on_material;
            }

            else{
                turned_on = false;
                GetComponent<MeshRenderer>().material = off_material;
            }
        }
    }

    public void StartSwitchCutscene(float cutscene_time){
        cutscene_bars.ActivateCutscene(cutscene_time);
    }
}
