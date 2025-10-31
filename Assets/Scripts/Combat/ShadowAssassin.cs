using UnityEngine;

[RequireComponent(typeof(CombatStateManager))]
public class ShadowAssassin : MonoBehaviour
{
    CombatStateManager combatStateManager;
    public float currentShadowMeter = 0f;
    public float maxShadowMeter = 100f;

    public bool shadowReady;
    
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
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED && shadowReady)
        {
            EnterShadowAssassin();
        }
    }

    private void EnterShadowAssassin()
    {
        
    }

    public void UpdateShadowMeter(float charge)
    {
        currentShadowMeter +=  charge;
        currentShadowMeter = Mathf.Clamp(currentShadowMeter, 0f, maxShadowMeter);

        if (currentShadowMeter >= maxShadowMeter)
        {
            shadowReady = true;
        }
    }
}
