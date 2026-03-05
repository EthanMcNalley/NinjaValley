using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShadowMaskIcon : MonoBehaviour
{
    public Image shadowMask;

    private float currentAlpha;
    public float changeSpeed = 0.4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowAssassinStarted;
        CombatEvents.ShadowAssassinEnded += OnShadowAssassinEnded;
    }

    void OnDisable()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowAssassinStarted;
        CombatEvents.ShadowAssassinEnded -= OnShadowAssassinEnded;
    }
    
    void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowAssassinStarted;
        CombatEvents.ShadowAssassinEnded -= OnShadowAssassinEnded;
    }

    void OnShadowAssassinStarted()
    {
        StartCoroutine(ChangeIcon(1f));
    }

    void OnShadowAssassinEnded()
    {
        StartCoroutine(ChangeIcon(0));
    }

    IEnumerator ChangeIcon(float target)
    {
        if (shadowMask.color.a == target) yield break;
        
        while (!Mathf.Approximately(shadowMask.color.a, target))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, target, changeSpeed * Time.deltaTime);
            
            Color newColor = shadowMask.color;
            newColor.a = currentAlpha;
            shadowMask.color = newColor;
            yield return null;
        }
    }
}
