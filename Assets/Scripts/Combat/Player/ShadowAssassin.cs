using UnityEngine;
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
    
    private Coroutine shadowCoroutine;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (combatStateManager == null)
        {
            combatStateManager = GetComponent<CombatStateManager>();
        }
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
    }

    private void EnterShadowAssassin()
    {

        if (!shadowReady || shadowActive)
        {
            return;
        }
            
        shadowReady = false;
        shadowActive = true;

        //Broadcast event so I don't have do something weird with the code for the CombatStateManager
        CombatEvents.RaiseShadowAssassinStarted();

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
        
        //revert any effects like screen and vfx stuff here if we have it...
        
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
    }
}
