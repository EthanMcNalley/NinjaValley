using UnityEngine;
using Unity.Cinemachine;

public class CutsceneBars : MonoBehaviour
{
    public static CutsceneState cutscene_state;
    public Animator cutscene_bars;
    public float active_timer = 0.0f;
    //public Animator UI_scroll_animator;
    public NewMovement player;
    public CanvasGroup hud;
    
    private CutsceneState previousState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cutscene_state = CutsceneState.INACTIVE;
        previousState = CutsceneState.INACTIVE;
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
            if (hud.alpha > 0)
            {
                player.DisableMovement();
                hud.alpha = Mathf.MoveTowards(hud.alpha, 0, 2f * Time.deltaTime);
            }
        }
        else{
            cutscene_state = CutsceneState.INACTIVE;
            if (hud.alpha < 1)
            {
                hud.alpha = Mathf.MoveTowards(hud.alpha, 1, 1.5f * Time.deltaTime);
            }
        }
        
        if (cutscene_state != previousState)
        {
            stateChanged(cutscene_state);
            previousState = cutscene_state;
        }

        /*if (cutscene_state == CutsceneState.ACTIVE){
            cutscene_bars.SetBool("Active", true);
            UI_scroll_animator.SetBool("Fold", true);
        }

        else{
            cutscene_bars.SetBool("Active", false);
            UI_scroll_animator.SetBool("Fold", false);
            //THIS WILL CAUSE ISSUES LATER ON, AND WILL LIKELY MAKE THE PLAYER CONTROLLABLE EVEN WHEN THEYRE NOT SUPPOSED TO, REMEMBER THIS WHEN THAT HAPPENS
        }*/
    }
    
    private void stateChanged(CutsceneState newState)
    {
        if (newState == CutsceneState.ACTIVE)
        {
            player.DisableMovement();
            Debug.Log("WDOODO");
            cutscene_bars.SetBool("Active", true);
            //UI_scroll_animator.SetBool("Fold", true);
        }
        else
        {
            CameraUtility.instance.InstantRecenter();
            player.EnableMovement();
            cutscene_bars.SetBool("Active", false);
            //UI_scroll_animator.SetBool("Fold", false);
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
