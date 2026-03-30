using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    public Animator blur_effect;
    public float unpause_delay = 0.7f;
    public TimeManager time_manager;
    public Image time_bar;
    public float fill_rate = 0.001f;
    private InputAction pauseAction;
    [SerializeField] NewMovement player;
    public enum UIState{
        INACTIVE,
        ACTIVE
    }

    public enum SettingsState{
        INACTIVE,
        ACTIVE
    }
    public GameObject cam;
    /*public Slider cam_sensitivity_slider_x;
    public Slider cam_sensitivity_slider_y;*/
    public Animator pause_animator;
    public Animator settings_animator;
    public Animator text_scroll_animator;
    public TMP_Text sign_text;

    [Header("Buttons")] 
    public EventSystem eventSystem;
    public GameObject textScrollButton, mainMenuButton, settingsButton;

    [Header("Menus")] 
    public GameObject textScroll;
    public GameObject mainMenu, settings;
    
    private bool button_pressed = false;

    // public enum UIType{
    //     PAUSE
    // }

    void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindFirstObjectByType<EventSystem>();
        }
    }
    
    void Start()
    {
        blur_effect.gameObject.SetActive(true);
		timer = duration;
        ui_state = UIState.INACTIVE;
        settings_state = SettingsState.INACTIVE;
        cam = Camera.main.gameObject;
        FadeOut();

        if (InputSystem.actions)
        {
            pauseAction = InputSystem.actions.FindAction("Menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.triggered){
            if(player.canMove){
                button_pressed = true;
            
                if (ui_state == UIState.INACTIVE  && settings_state == SettingsState.INACTIVE){
                    OpenMenu();
                }
                else{
                    CloseMenu();
                }
            }
        }
        /*else if (pauseAction.triggered && settings_state == SettingsState.ACTIVE)
        {
            settings_animator.SetTrigger("SlideOut");
            settings_state =  SettingsState.INACTIVE;
        }*/
        
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED){
            time_bar.fillAmount = (time_manager.time_slowed_down - time_manager.time_timer) / time_manager.time_slowed_down;
        }

        else{
            time_bar.fillAmount = time_bar.fillAmount + (Time.deltaTime / time_manager.refresh_time);
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
    /*public void ChangeSensitivityX(){
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
    }*/

    public void OpenMenu(){
        // timer = 0.0f;
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(mainMenuButton);
        Time.timeScale = 0.0f;
        FadeIn();
        ui_state = UIState.ACTIVE;
        pause_animator.SetBool("Idle", false);
        pause_animator.SetBool("Pause", true);
        button_pressed = false;
    }

    public void CloseMenu(){
        // timer = 0.0f;
        if (settings_state == SettingsState.INACTIVE){
            FadeOut();
            ui_state = UIState.INACTIVE;
            eventSystem.SetSelectedGameObject(null);
            pause_animator.SetBool("Idle", false);
            pause_animator.SetBool("Pause", false);
            ResetTime();
        }
        else{
            mainMenu.SetActive(true);
            pause_animator.SetBool("Idle", true);
            pause_animator.SetBool("Pause", true);
            eventSystem.SetSelectedGameObject(mainMenuButton);
            settings_state = SettingsState.INACTIVE;
            settings_animator.SetTrigger("SlideOut");
        }
        button_pressed = false;
        //Invoke("ResetTime", unpause_delay);
    }

    public void SettingsMenu(){
        if (ui_state == UIState.ACTIVE){
            if (settings_state == SettingsState.INACTIVE){
                mainMenu.SetActive(false);
                settings_state = SettingsState.ACTIVE;
                settings_animator.SetBool("Settings", true);
            }
        }
    }

    public void OpenTextScrollMenu(string message){
        if (ui_state == UIState.INACTIVE)
        {
            textScroll.SetActive(true);
            FadeIn();
            sign_text.text = message;
            eventSystem.SetSelectedGameObject(textScrollButton);
            text_scroll_animator.SetBool("Active", true);
            ui_state = UIState.ACTIVE;
            Time.timeScale = 0.0f;
        }
    }

    public void CloseTextScrollMenu(){
        if (ui_state == UIState.ACTIVE){
            Time.timeScale = 1.0f;
            FadeOut();
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
        blur_effect.SetBool("Blur", true);
    }

    public void FadeOut(){
        blur_effect.SetBool("Blur", false);
    }
    public void TextScrollFalse() => textScroll.SetActive(false);

    public void SetSettingsState(bool state)
    {
        settings_state = state? SettingsState.ACTIVE : SettingsState.INACTIVE;
    }
}
