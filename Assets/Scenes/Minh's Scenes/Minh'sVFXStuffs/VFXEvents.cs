using UnityEngine;
using UnityEngine.VFX;
public class VFXEvents : MonoBehaviour
{
    public VisualEffect vfx;
    public float timer = 0f, maxTimer = 8f;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        timer = maxTimer;
        vfx.SendEvent("OnPlay");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       // Data is copied from eventAttribute, so this object can be used again
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            timer = maxTimer;
            vfx.SendEvent("OnStop");
            this.gameObject.SetActive(false);
            
        }   

    }
}
