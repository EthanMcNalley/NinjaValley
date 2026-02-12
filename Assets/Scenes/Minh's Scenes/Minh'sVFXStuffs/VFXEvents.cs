using UnityEngine;
using UnityEngine.VFX;
public class VFXEvents : MonoBehaviour
{
    public VisualEffect vfx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vfx.SendEvent("OnPlay");
    }

    // Update is called once per frame
    void Update()
    {
        // Data is copied from eventAttribute, so this object can be used again
        
    }
}
