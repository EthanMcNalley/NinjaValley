using UnityEngine;
using UnityEngine.Splines;

public class SplineSpeedAdjust : MonoBehaviour
{
    SplineAnimate splineAnim;
    float original_speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = GetComponent<SplineAnimate>();
        original_speed = splineAnim.MaxSpeed;
    }
    private void UpdatePathSpeed()
    {
        float prevProgress = splineAnim.NormalizedTime;
        splineAnim.MaxSpeed = original_speed * TimeManager.slowed_amount;
        splineAnim.NormalizedTime = prevProgress;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePathSpeed();
    }
}
