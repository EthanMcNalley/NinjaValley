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
    [SerializeField]private ScriptableRendererFeature shadowVisual;
    
    [Header("Shader Control")]
    public Material shader;
    public float shaderDuration = 1f;
    public float shaderPower = 2.41f;
    private float shaderTime;
    private bool shaderFading;
    private bool shaderTargetState;
    
    public Material time_slow_material;
    private Material instance_material_expand;
    public Material vignette_material;
    private Material instance_vignette_material;
    public FullScreenPassRendererFeature shadowSlowRenderer;
    public FullScreenPassRendererFeature vignette;
    public float material_rate = 5.0f;
    public float max_size = 3.0f;
    
    private float time_size = 0.0f;
    
    public float fadeSpeed = 2.0f;
    private float effectStrength = 0f;

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

        if (InputSystem.actions)
        {
            shadowAction = InputSystem.actions.FindAction("Shadow");
            if (shadowAction != null)
            {
                shadowAction.Enable();
            }
        }
        
        instance_material_expand = new Material(time_slow_material);
        time_size = 0f;
        instance_material_expand.SetFloat("_WipeSize", time_size);
        
        instance_vignette_material = new Material(vignette_material);
        
        shadowSlowRenderer.passMaterial = instance_material_expand;
        vignette.passMaterial =  instance_vignette_material;
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

        if (shadowActive)
        {
            if (time_size < max_size)
            {
                time_size += Time.deltaTime * material_rate;
                time_size = Mathf.Min(time_size, max_size);
                instance_material_expand.SetFloat("_WipeSize", time_size);
            }
            
            if (effectStrength < 1f)
            {
                effectStrength += Time.deltaTime * fadeSpeed;
                instance_vignette_material.SetFloat("_EffectStrength", effectStrength);
            }
        }
        else 
        {
            if (time_size > 0)
            {
                time_size -= Time.deltaTime * material_rate;
                time_size = Mathf.Max(time_size, 0f);
                instance_material_expand.SetFloat("_WipeSize", time_size);
            }
        
            if (effectStrength > 0)
            {
                effectStrength -= Time.deltaTime * fadeSpeed;
                instance_vignette_material.SetFloat("_EffectStrength", effectStrength);
            }
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
}
