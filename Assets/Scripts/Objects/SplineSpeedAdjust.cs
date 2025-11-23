using UnityEngine;
using UnityEngine.Splines;

public class SplineSpeedAdjust : MonoBehaviour
{
    SplineAnimate splineAnim;
    public float original_speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnim = GetComponent<SplineAnimate>();
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

    public void ChangeOriginalSpeed(float new_speed){
        original_speed = new_speed;
    }
}
