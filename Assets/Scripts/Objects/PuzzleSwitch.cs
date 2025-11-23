using UnityEngine;
using UnityEngine.Events;

public class PuzzleSwitch : MonoBehaviour
{
    public UnityEvent switch_action;
    public bool done = false;
    public Material done_material;
    public Light done_light;
    public Color done_light_color;
    CutsceneBars cutscene_bars;
    void Start(){
        cutscene_bars = GameObject.FindGameObjectWithTag("UIManager").GetComponent<CutsceneBars>();
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("AttackHitBox"))
        {
            if (!done){
                switch_action.Invoke();
                done = true;
            }

            if (done){
                GetComponent<MeshRenderer>().material = done_material;
                done_light.color = done_light_color;
            }
        }
    }

    public void StartSwitchCutscene(float cutscene_time){
        cutscene_bars.ActivateCutscene(cutscene_time);
    }
}
