using UnityEngine;
using UnityEngine.VFX;
public class VFXEvents : MonoBehaviour
{
    public VisualEffect vfx;
    public float vfxTimer = 0f, vfxMaxTimer = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        vfx.SendEvent("OnPlay");
        vfxTimer = vfxMaxTimer;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vfxTimer -= Time.deltaTime;
        if (vfxTimer <= 0f)
        {
            vfx.Stop();
            gameObject.SetActive(false);
        }
        /*if (vfx.aliveParticleCount == 0)
        {
            vfx.Stop();
            Destroy(this.gameObject);
        }*/
    }
}
