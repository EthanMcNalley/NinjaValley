using UnityEngine;

public class CutsceneBars : MonoBehaviour
{
    public static CutsceneState cutscene_state;
    public Animator cutscene_bars;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cutscene_state = CutsceneState.INACTIVE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)){
            cutscene_state = CutsceneState.ACTIVE;
        }

        if (Input.GetKeyDown(KeyCode.V)){
            cutscene_state = CutsceneState.INACTIVE;
        }

        if (cutscene_state == CutsceneState.ACTIVE){
            cutscene_bars.SetBool("Active", true);
        }

        else{
            cutscene_bars.SetBool("Active", false);
        }
    }

    public enum CutsceneState{
        ACTIVE,
        INACTIVE
    }
}
