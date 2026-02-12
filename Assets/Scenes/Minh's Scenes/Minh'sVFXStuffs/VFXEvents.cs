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
        /*if (vfx.aliveParticleCount == 0)
        {
            vfx.Stop();
            Destroy(this.gameObject);
        }*/
    }
}
