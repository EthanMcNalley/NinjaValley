using UnityEngine;
using FMODUnity;

public class PlayOneShots : MonoBehaviour
{
    public EventReference[] sound_effect;    
    public void PlayTheOneShot(int num){
        AudioManager.instance.PlayOneShot(sound_effect[num], transform.position);
    }
}
