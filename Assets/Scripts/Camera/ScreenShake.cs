using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header ("Screen Shake")] 
    public float shakeDuration = 0.3f;
    public AnimationCurve shakeCurve;
        
    
    public void DoScreenShake(float duration, AnimationCurve curve)
    {
        shakeDuration = duration;
        shakeCurve = curve;
        Debug.Log("Shake");
        StartCoroutine(IEnumScreenShake());
    }
        
    public IEnumerator IEnumScreenShake()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
    
        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = shakeCurve.Evaluate(elapsedTime / shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
            
        transform.position = startPos;
    }
}
