using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CombatStateManager))]
public class ShadowAssassin : MonoBehaviour
{
    CombatStateManager combatStateManager;
    public float currentShadowMeter = 0f;
    public float maxShadowMeter = 100f;

    public float shadowAssassinDuration = 6f;
    public float shadowAssassinDamagePercentage = 0.3f;

    public bool shadowReady;
    public bool shadowActive;
    
    public Slider shadowBarSlider;
    private Image shadowBarImage;
    private float ratio;
    private float timer;

    private InputAction shadowAction;
    private Coroutine shadowCoroutine;
    
    [Header("Visual Stuff")]
    public GameObject volume;
    [SerializeField]private ScriptableRendererFeature shadowVisual;
    
    [Header("Shader Control")]
    public Material shader;
    public float shaderDuration = 1f;
    public float shaderPower = 2.41f;
    private float shaderTime;
    private bool shaderFading;
    private bool shaderTargetState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (combatStateManager == null)
        {
            combatStateManager = GetComponent<CombatStateManager>();
        }
        timer = shadowAssassinDuration;
        
        ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;
        
        shadowVisual.SetActive(false);

        if (InputSystem.actions)
        {
            shadowAction = InputSystem.actions.FindAction("Shadow");
            if (shadowAction != null)
            {
                shadowAction.Enable();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shadowAction.triggered && shadowReady && !shadowActive)
        {
            EnterShadowAssassin();
        }
        
        /*else if (shadowActive && shadowAction.triggered && TimeManager.time_state == TimeManager.TimeState.SLOWED)
        {
            ExitShadowAssassin();
        }*/

        if (shadowActive)
        {
            timer -= Time.deltaTime;
            ratio = Mathf.Clamp01( timer / shadowAssassinDuration);
            shadowBarSlider.value = ratio;
        }
        
        /*ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;*/
    }

    private void EnterShadowAssassin()
    {
        if (!shadowReady || shadowActive)
        {
            return;
        }
            
        shadowReady = false;
        shadowActive = true;
        timer = shadowAssassinDuration;
        AudioManager.instance.SetSlowTime(1f);
        UpdateVisuals();
        
        //Broadcast event so I don't have do something weird with the code for the CombatStateManager
        CombatEvents.RaiseShadowAssassinStarted();

        shadowCoroutine = StartCoroutine(ShadowAssassinTimer());
    }
    
    private IEnumerator ShadowAssassinTimer()
    {
        yield return new WaitForSecondsRealtime(shadowAssassinDuration);
        EndShadowAssassin();
    }

    private void ExitShadowAssassin()
    {
        if (!shadowActive)
        {
            return;
        }

        if (shadowCoroutine != null)
        {
            StopCoroutine(shadowCoroutine);
            shadowCoroutine = null;
        }
        
        EndShadowAssassin();
    }

    private void EndShadowAssassin()
    {
        shadowActive = false;
        shadowReady = false;
        currentShadowMeter = Mathf.Clamp01( timer / shadowAssassinDuration) * 100;
        //revert any effects like screen and vfx stuff here if we have it...
        
        ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;
        AudioManager.instance.SetSlowTime(0f);
        UpdateVisuals();
        
        CombatEvents.RaiseShadowAssassinEnded();
    }

    public void UpdateShadowMeter(float charge)
    {
        if (!shadowActive)
        {
            currentShadowMeter += charge;
            currentShadowMeter = Mathf.Clamp(currentShadowMeter, 0f, maxShadowMeter);
        }
        else
        {
            return;
        }

        if (currentShadowMeter >= maxShadowMeter)
        {
            shadowReady = true;
        }
        
        ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;
    }

    private void UpdateVisuals()
    {
        if (shadowActive)
        {
            volume.SetActive(true);
            shadowVisual.SetActive(true);
        }
        else
        {
            volume.SetActive(false);
            shadowVisual.SetActive(false);
        }
    }
}
