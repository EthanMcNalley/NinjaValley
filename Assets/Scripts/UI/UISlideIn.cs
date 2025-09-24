using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerInput player_controls;
    
    public float timer;
	public float duration = 2.0f;
	public float speed = 3.5f;
    public static UIState ui_state;
    //public UIType ui_type;
    public float delay;
    public static UIManager instance { get; private set; }
    public static bool activate = false;
    public Image blur_effect;
    public float unpause_delay = 0.7f;
    
    private InputAction menuAction;
    
    public enum UIState{
        INACTIVE,
        ACTIVE
    }

    public Animator pause_animator;

    // public enum UIType{
    //     PAUSE
    // }

    void Start()
    {
        menuAction = InputSystem.actions.FindAction("UI/Menu");
        
        blur_effect.gameObject.SetActive(true);
		timer = duration;
        ui_state = UIState.INACTIVE;
        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (menuAction.triggered){
            if (ui_state == UIState.INACTIVE){
                OpenMenu();
            }

            else{
                CloseMenu();
            }
        }
        // if (timer <= duration)
        // {
        //     timer += Time.deltaTime;

        //     float percentage = timer / duration;

        //     if (ui_state == UIState.ACTIVE)
        //     {
        //         gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(original_position, active_position, Mathf.SmoothStep(0, 1, percentage * speed));
        //     }
        //     else
        //     {
        //         gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(active_position, original_position, Mathf.SmoothStep(0, 1, percentage * speed));
        //     }
        // }
    }

    public void OpenMenu(){
        // timer = 0.0f;
        Time.timeScale = 0.0f;
        FadeIn();
        ui_state = UIState.ACTIVE;
        pause_animator.SetBool("Pause", true);
        
    }

    public void CloseMenu(){
        // timer = 0.0f;
        FadeOut();
        ui_state = UIState.INACTIVE;
        pause_animator.SetBool("Pause", false);
        ResetTime();
        //Invoke("ResetTime", unpause_delay);
    }

    public void ResetTimer(){
        timer = 0.0f;
    }

    public void ResetTime(){
        Time.timeScale = 1.0f;
    }

    public void FadeIn(){
        blur_effect.CrossFadeAlpha(1, 0.5f, true);
    }

    public void FadeOut(){
        blur_effect.CrossFadeAlpha(0, 0.25f, true);
    }
}
