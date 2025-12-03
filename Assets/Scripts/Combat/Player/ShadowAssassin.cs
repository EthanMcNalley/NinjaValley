using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    
    private Coroutine shadowCoroutine;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (combatStateManager == null)
        {
            combatStateManager = GetComponent<CombatStateManager>();
        }
        timer = shadowAssassinDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED && shadowReady && !shadowActive)
        {
            EnterShadowAssassin();
        }

        if (shadowActive && TimeManager.time_state == TimeManager.TimeState.NORMAL)
        {
            ExitShadowAssassin();
        }

        if (TimeManager.time_state == TimeManager.TimeState.SLOWED && shadowActive)
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
        currentShadowMeter = 0f;
        //revert any effects like screen and vfx stuff here if we have it...
        
        ratio = Mathf.Clamp01(currentShadowMeter / maxShadowMeter);
        shadowBarSlider.value = ratio;
        
        CombatEvents.RaiseShadowAssassinEnded();
    }

    public void UpdateShadowMeter(float charge)
    {
        if (TimeManager.time_state == TimeManager.TimeState.NORMAL)
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
