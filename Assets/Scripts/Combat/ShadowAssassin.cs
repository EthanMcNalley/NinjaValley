using UnityEngine;

[RequireComponent(typeof(CombatStateManager))]
public class ShadowAssassin : MonoBehaviour
{
    CombatStateManager combatStateManager;
    public float currentShadowMeter = 0f;
    public float maxShadowMeter = 100f;
    
    public bool shadowReady {get; private set;}
    
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
