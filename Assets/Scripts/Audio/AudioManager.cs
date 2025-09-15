using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    
    public bool needMusic;
    
    public EventInstance soundToStop;
    public EventInstance musicEventInstance;
    
    private PlaySound psound;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (needMusic)
        {
            InitializeMusic(FmodEvents.instance.music);
        }
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }
    
    public void PlayOneShot(EventReference clip, Vector3 position)
    {
        RuntimeManager.PlayOneShot(clip, position);
    }

    public EventInstance CreateEventInstance(EventReference eventRef)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
        return eventInstance;
    }

    public void SetMusicArea(MusicEnum area)
    {
        musicEventInstance.setParameterByName("area", (float) area);
    }
    
    public void StopEventInstance(EventInstance eventInstance)
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    
    public void DisableSound()
    {
        psound = GetComponent<PlaySound>();
        EventInstance eventInstance = psound.PlaySoundEvent;
        instance.StopEventInstance(eventInstance);
    }
    
    public bool IsPlaying(EventInstance instance) {
        PLAYBACK_STATE state;   
        instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }
}
