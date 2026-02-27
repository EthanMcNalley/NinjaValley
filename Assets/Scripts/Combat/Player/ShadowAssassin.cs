using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using FMODUnity;
using UnityEngine.Rendering;
//using EventReference = FMODUnity.EventReference;

[RequireComponent(typeof(CombatStateManager))]
public class ShadowAssassin : MonoBehaviour
{
    CombatStateManager combatStateManager;
    public TimeManager timeManager;
    public float currentShadowMeter = 0f;
    public float maxShadowMeter = 100f;

    public float shadowAssassinDuration = 6f;
    public float shadowAssassinDamagePercentage = 0.3f;

    public bool shadowReady;
    private bool shadowActive;
    private bool timeSlowed;
    private bool inCombat;
    
    public Slider shadowBarSlider;
    public GameObject ninetails, boarder, insignia;
    private Image ninetailsImage, boarderImage;
    //private Image shadowBarImage;
    private float ratio;
    private float timer;

    private InputAction shadowAction;
    private Coroutine shadowCoroutine;
    
    [Header("Visual Stuff")]
    [SerializeField]private ScriptableRendererFeature shadowVisual;
    public GameObject terrainScannerPrefab;
    public float scanDurration = 6f;
    public float scanSize = 700;
    
    public SkinnedMeshRenderer characterRenderer;
    public Material maskFlareMaterial;
    private Material[] saveMaterials;
    public Volume shadowVolume;
    
    public List<GameObject> CloneVFX;
    
    
    [Header("Shader Control")]
    public Material shader;
    public float shaderDuration = 1f;
    public float shaderPower = 2.41f;
    private float shaderTime;
    private bool shaderFading;
    private bool shaderTargetState;
    
    public Material purple_time_slow_material;
    private Material time_slow_material, instance_purple_material_expand;
    public Material vignette_material;
    private Material instance_vignette_material;
    public FullScreenPassRendererFeature shadowSlowRenderer;
    public FullScreenPassRendererFeature vignette;
    public float material_rate = 5.0f;
    public float max_size = 3.0f;
    
    private float time_size = 0.0f;
    private float clear_time_size = 0.0f;
    
    public float fadeSpeed = 2.0f;
    private float effectStrength = 0f;

    [Header("Audio")] public EventReference shadowEnterSound;

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
            shadowAction = InputSystem.actions.FindAction("TimeSlow");
            if (shadowAction != null)
            {
                shadowAction.Enable();
            }
        }
        
        ninetailsImage = ninetails.GetComponent<Image>();
        boarderImage = boarder.GetComponent<Image>();
        ninetailsImage.fillAmount = 0f;
        boarderImage.fillAmount = 0f;

        time_slow_material = timeManager.GetMaterial();
        clear_time_size = 0f;
        
        saveMaterials = (Material[])characterRenderer.materials.Clone();
        
        if (currentShadowMeter >= maxShadowMeter)
        {
            shadowReady = true;
            insignia.SetActive(true);
        }
    }

    private void Awake()
    {
        instance_purple_material_expand = new Material(purple_time_slow_material);
        instance_purple_material_expand.SetFloat("_WipeSize", time_size);
        
        instance_vignette_material = new Material(vignette_material);
        shadowSlowRenderer.passMaterial = instance_purple_material_expand;
        vignette.passMaterial =  instance_vignette_material;
    }

    private void OnEnable()
    {
        CombatEvents.PlayerInCombat += OnPlayerCombat;
        CombatEvents.PlayerInCombatEnded += OnPlayerCombatEnded;
        CombatStateManager.PlayerAttack += OnPlayerAttack;
    }

    private void OnDestroy()
    {
        CombatEvents.PlayerInCombat -= OnPlayerCombat;
        CombatEvents.PlayerInCombatEnded -= OnPlayerCombatEnded;
        CombatStateManager.PlayerAttack -= OnPlayerAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if (shadowAction.triggered && shadowReady && !shadowActive && TimeManager.time_state == TimeManager.TimeState.SLOWED && inCombat)
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
            ninetailsImage.fillAmount = ratio;
            boarderImage.fillAmount = ratio;
        }

        ScreenEffect();
        
        /*ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;*/
    }

    private void ScreenEffect()
    {
        if (shadowActive)
        {
            if (clear_time_size < max_size)
            {
                /*time_size += Time.deltaTime * material_rate;
                time_size = Mathf.Min(time_size, max_size);
                instance_purple_material_expand.SetFloat("_WipeSize", time_size);*/
                clear_time_size += Time.deltaTime * material_rate;
                clear_time_size = Mathf.Min(clear_time_size, max_size);
                time_slow_material.SetFloat("_ClearSize", clear_time_size);
                
                shadowVolume.weight = Mathf.MoveTowards(shadowVolume.weight, 1f, Time.deltaTime * (material_rate/max_size));
            }
            
            if (effectStrength < 1f)
            {
                effectStrength += Time.deltaTime * fadeSpeed;
                instance_vignette_material.SetFloat("_EffectStrength", effectStrength);
            }
        }
        else 
        {
            if (clear_time_size > 0)
            {
                /*time_size -= Time.deltaTime * material_rate;
                time_size = Mathf.Max(time_size, 0f);
                instance_purple_material_expand.SetFloat("_WipeSize", time_size);*/
                clear_time_size -= Time.deltaTime * material_rate;
                clear_time_size = Mathf.Max(clear_time_size, 0);
                time_slow_material.SetFloat("_ClearSize", clear_time_size);
                
                shadowVolume.weight = Mathf.MoveTowards(shadowVolume.weight, 0f, Time.deltaTime * (material_rate/max_size));
            }
        
            if (effectStrength > 0)
            {
                effectStrength -= Time.deltaTime * fadeSpeed;
                instance_vignette_material.SetFloat("_EffectStrength", effectStrength);
            }
        }
    }
    
    private void EnterShadowAssassin()
    {
        if (!shadowReady || shadowActive)
        {
            return;
        }
        
        AudioManager.instance.PlayOneShot(shadowEnterSound,  transform.position);
        
        shadowReady = false;
        shadowActive = true;
        timer = shadowAssassinDuration;
        insignia.SetActive(false);
        shadowBarSlider.value = 0f;
        //AudioManager.instance.SetSlowTime(1f);
        
        var Materials = (Material[])saveMaterials.Clone();
        Materials[^1] = maskFlareMaterial;
        characterRenderer.materials = Materials;
        
        //Broadcast event so I don't have do something weird with the code for the CombatStateManager
        CombatEvents.RaiseShadowAssassinStarted();
        InstantiateTerrainScanner();
        
        ninetailsImage.fillAmount = 1f;
        boarderImage.fillAmount = 1f;

        shadowCoroutine = StartCoroutine(ShadowAssassinTimer());
    }
    
    private IEnumerator ShadowAssassinTimer()
    {
        yield return new WaitForSeconds(shadowAssassinDuration);
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
        //AudioManager.instance.SetSlowTime(0f);
        
        Material[] materials = characterRenderer.materials;
        Array.Resize(ref materials, materials.Length - 1);
        
        characterRenderer.materials = (Material[])saveMaterials.Clone();
        
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
            insignia.SetActive(true);
            if (inCombat)
            {
                if (!Mathf.Approximately(boarderImage.fillAmount, 1f)) boarderImage.fillAmount = 1f;
            }
        }
        
        ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;
    }

    private void OnPlayerAttack(AttackData data)
    {
        if (data.stateID == AttackData.CombatStateID.GroundAttack3 && shadowActive)
        {
            foreach (var clone in CloneVFX)
            {
                clone.gameObject.SetActive(true);
            }
        }
    }

    private void OnPlayerCombat()
    {
        inCombat = true;
        if (!shadowReady) return;
        
        boarderImage.fillAmount = 1f;
    }

    private void OnPlayerCombatEnded()
    {
        inCombat = false;
        if (shadowActive) return;

        if (boarderImage != null)
        {
            boarderImage.fillAmount = 0;
        }
    }
    
    void InstantiateTerrainScanner()
    {
        GameObject terrainScanner = Instantiate(terrainScannerPrefab, combatStateManager.transform.position, Quaternion.identity);
        ParticleSystem ps = terrainScanner.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var main = ps.main;
            main.startLifetime = scanDurration;
            main.startSize = scanSize;
        }
        else
        {
            return;
        }
        
        Destroy(terrainScanner, scanDurration + 1);
    }

    public bool getShadowActive()
    {
        return shadowActive;
    }

    public float getTimer()
    {
        return timer;
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShadowEssence"))
        {
            
        }
    }*/
}
