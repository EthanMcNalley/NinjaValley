using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float timer;
	public float duration = 2.0f;
	public float speed = 3.5f;
    public static UIState ui_state;
    public static SettingsState settings_state;
    //public UIType ui_type;
    public float delay;
    public static UIManager instance { get; private set; }
    public static bool activate = false;
    public Image blur_effect;
    public float unpause_delay = 0.7f;
    public TimeManager time_manager;
    public Image time_bar;
    public float fill_rate = 0.001f;
    public enum UIState{
        INACTIVE,
        ACTIVE
    }

    public enum SettingsState{
        INACTIVE,
        ACTIVE
    }
    public GameObject cam;
    public Slider cam_sensitivity_slider_x;
    public Slider cam_sensitivity_slider_y;
    public Animator pause_animator;
    public Animator settings_animator;
    public Animator text_scroll_animator;

    // public enum UIType{
    //     PAUSE
    // }

    void Start()
    {
        blur_effect.gameObject.SetActive(true);
		timer = duration;
        ui_state = UIState.INACTIVE;
        settings_state = SettingsState.INACTIVE;
        cam = Camera.main.gameObject;
        FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            if (ui_state == UIState.INACTIVE){
                OpenMenu();
            }

            else{
                CloseMenu();
            }
        }
        
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED){
            time_bar.fillAmount = (time_manager.time_slowed_down - time_manager.time_timer) / time_manager.time_slowed_down;
        }

        else{
            time_bar.fillAmount = time_bar.fillAmount + fill_rate;
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
    public void ChangeSensitivityX(){
        foreach (var controller in cam.GetComponent<CinemachineInputAxisController>().Controllers){
            if (controller.Name == "Gain"){
                controller.Input.Gain = cam_sensitivity_slider_x.value;
            }
        }
    }

    public void ChangeSensitivityY(){
        foreach (var controller in cam.GetComponent<CinemachineInputAxisController>().Controllers){
            if (controller.Name == "Gain"){
                controller.Input.Gain = cam_sensitivity_slider_y.value * -1;
            }
        }
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
        if (settings_state == SettingsState.INACTIVE){
            FadeOut();
            ui_state = UIState.INACTIVE;
            pause_animator.SetBool("Pause", false);
            ResetTime();
        }
        //Invoke("ResetTime", unpause_delay);
    }

    public void SettingsMenu(){
        if (ui_state == UIState.ACTIVE){
            if (settings_state == SettingsState.INACTIVE){
                settings_state = SettingsState.ACTIVE;
                settings_animator.SetBool("Settings", true);
            }

            else{
                settings_state = SettingsState.INACTIVE;
                settings_animator.SetBool("Settings", false);
            }
        }
    }

    public void OpenTextScrollMenu(){
        if (ui_state == UIState.INACTIVE){
            text_scroll_animator.SetBool("Active", true);
            ui_state = UIState.ACTIVE;
        }
    }

    public void CloseTextScrollMenu(){
        if (ui_state == UIState.ACTIVE){
            text_scroll_animator.SetBool("Active", false);
            ui_state = UIState.INACTIVE;
        }
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
