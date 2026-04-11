using UnityEngine;
using FMODUnity;

public class PlayOneShotAudio : MonoBehaviour
{
    public bool play_on_awake = false;
    public EventReference sound_effect;    
    void Start()
    {
        if (play_on_awake)
        {
            if (sound_effect.IsNull)
            {
                return;
            }
            
            AudioManager.instance.PlayOneShot(sound_effect, transform.position);
        }
    }
    public void PlayTheOneShot(){
        if (sound_effect.IsNull)
        {
            return;
        }
        
        AudioManager.instance.PlayOneShot(sound_effect, transform.position);
    }
}
