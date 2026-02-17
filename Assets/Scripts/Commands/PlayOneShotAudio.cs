using UnityEngine;
using FMODUnity;

public class PlayOneShotAudio : MonoBehaviour
{
    public EventReference sound_effect;    
    public void PlayTheOneShot(){
        if (sound_effect.IsNull)
        {
            return;
        }
        
        AudioManager.instance.PlayOneShot(sound_effect, transform.position);
    }
}
