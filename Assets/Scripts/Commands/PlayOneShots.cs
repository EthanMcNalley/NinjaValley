using UnityEngine;
using FMODUnity;

public class PlayOneShots : MonoBehaviour
{
    public EventReference[] sound_effect;    
    public void PlayTheOneShot(int num){
        if (!(sound_effect.Length == 0))
        {
            AudioManager.instance.PlayOneShot(sound_effect[num], transform.position);
        }
    }
}
