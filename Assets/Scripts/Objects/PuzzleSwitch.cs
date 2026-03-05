using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleSwitch : MonoBehaviour
{
    public UnityEvent switch_action;
    public bool done = false;
    public Material base_material;
    public Color base_color;
    public Material done_material;
    public Light done_light;
    public Color done_light_color;
    CutsceneBars cutscene_bars;
    public GameObject hit_particle;
    public EventReference switch_hit_sfx;
    void Start(){
        cutscene_bars = GameObject.FindGameObjectWithTag("UIManager").GetComponent<CutsceneBars>();
    }

    void Update()
    {
        if (!done)
        {
            GetComponent<MeshRenderer>().material = base_material;
            done_light.color = base_color;
        }
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("AttackHitBox"))
        {
            if (!done){
                switch_action.Invoke();
                done = true;
                AudioManager.instance.PlayOneShot(switch_hit_sfx, transform.position);
            }

            if (done){
                GetComponent<MeshRenderer>().material = done_material;
                done_light.color = done_light_color;
            }

            // Rigidbody sword_rb = collider.GetComponent<Rigidbody>();
            // Vector3 particle_direction = sword_rb.linearVelocity.normalized;
            // Quaternion correct_roto = Quaternion.LookRotation(-particle_direction);
            Quaternion correct_roto = Quaternion.LookRotation(-(collider.ClosestPoint(transform.position) - collider.transform.position).normalized);
            Instantiate(hit_particle, collider.ClosestPoint(transform.position), correct_roto);
        }
    }

    public void StartSwitchCutscene(float cutscene_time){
        cutscene_bars.ActivateCutscene(cutscene_time);
    }

    public void RefreshSwitch()
    {
        done = false;
    }
}
