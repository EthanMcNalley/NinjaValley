using UnityEngine;
using UnityEngine.VFX;

public class bloodInk : MonoBehaviour
{
    public VisualEffect bloodInkFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bloodInkFX = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.time_state == TimeManager.TimeState.SLOWED)
        {
            bloodInkFX.playRate = 0.1f;
        }
        else
        {
            bloodInkFX.playRate = 1f;
        }
    }
}
