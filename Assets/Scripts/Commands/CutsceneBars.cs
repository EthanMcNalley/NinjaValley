using UnityEngine;

public class CutsceneBars : MonoBehaviour
{
    public static CutsceneState cutscene_state;
    public Animator cutscene_bars;
    public float active_timer = 0.0f;
    public Animator UI_scroll_animator;
    public NewMovement player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cutscene_state = CutsceneState.INACTIVE;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.C)){
        //     cutscene_state = CutsceneState.ACTIVE;
        // }

        // if (Input.GetKeyDown(KeyCode.V)){
        //     cutscene_state = CutsceneState.INACTIVE;
        // }

        if (active_timer > 0){
            active_timer = active_timer - Time.deltaTime;
            cutscene_state = CutsceneState.ACTIVE;
            player.DisableMovement();
        }

        else{
            cutscene_state = CutsceneState.INACTIVE;
            player.EnableMovement();
        }

        if (cutscene_state == CutsceneState.ACTIVE){
            cutscene_bars.SetBool("Active", true);
            UI_scroll_animator.SetBool("Fold", true);
        }

        else{
            cutscene_bars.SetBool("Active", false);
            UI_scroll_animator.SetBool("Fold", false);
            //THIS WILL CAUSE ISSUES LATER ON, AND WILL LIKELY MAKE THE PLAYER CONTROLLABLE EVEN WHEN THEYRE NOT SUPPOSED TO, REMEMBER THIS WHEN THAT HAPPENS
        }
    }

    public void ActivateCutscene(float bar_time){
        active_timer = bar_time;
    }

    public enum CutsceneState{
        ACTIVE,
        INACTIVE
    }
}
