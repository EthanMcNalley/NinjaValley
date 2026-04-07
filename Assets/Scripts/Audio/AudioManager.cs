using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.AI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    
    public bool needMusic;
    public bool need_footsteps = true;
    public MusicEnum startingMusic = 0;
    
    public EventInstance soundToStop;
    public EventInstance musicEventInstance;
    
    private EventInstance CurrentSound;
    public string masterVCAPath;
    public VCA vca;
    public float initialVolume;
    
    //footstep stuff
    public EventInstance footstepEventInstance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (needMusic)
        {
            InitializeMusic(FmodEvents.instance.music);
        }
        
        if (need_footsteps)
        {
            InitializeFootstep(FmodEvents.instance.footstep);   
        }
        
        if (string.IsNullOrEmpty(masterVCAPath)) return;
        vca = RuntimeManager.GetVCA(masterVCAPath);
        if (vca.isValid())
        {
            vca.setVolume(initialVolume);
        }
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
        SetMusicArea(startingMusic);
    }
    
    private void InitializeFootstep(EventReference eventReference)
    {
        footstepEventInstance = CreateEventInstance(eventReference);
        //footstepEventInstance.start();
        //SetMusicArea(startingMusic);
    }
    
    public void PlayOneShot(EventReference clip, Vector3 position)
    {
        RuntimeManager.PlayOneShot(clip, position);
    }
    
    public void PlayOneShot(string clip, Vector3 position)
    {
        RuntimeManager.PlayOneShot(clip, position);
    }
    
    public IEnumerator PlaySound(EventReference clip, GameObject gameObject, bool waitToEnd)
    {
        CurrentSound = CreateEventInstance(clip);
        RuntimeManager.AttachInstanceToGameObject(CurrentSound, gameObject, false);
        CurrentSound.getDescription(out EventDescription description);

        int soundLength;
        description.getLength(out soundLength);
        //UnityEngine.Debug.Log(soundLength);
        CurrentSound.start();

        if (waitToEnd)
        {
            yield return new WaitForSeconds(soundLength / 1000f);
        }
    }

    //Release EventInstance to save resources
    public void ReleaseEventInstance()
    {
        if (CurrentSound.isValid())
        {
            CurrentSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CurrentSound.release();
        }
    }

    //To check how long an audio clip would play for
    public float GetSoundLengthInSeconds(EventReference clip)
    {
        CurrentSound = CreateEventInstance(clip);
        CurrentSound.getDescription(out EventDescription description);
        
        description.getLength(out var soundLength);
        //UnityEngine.Debug.Log(soundLength);
        CurrentSound.release();
        return (soundLength / 1000f); //in ms so have to divide 1000
    }

    private EventInstance CreateEventInstance(EventReference eventRef)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
        return eventInstance;
    }

    public void SetMusicArea(MusicEnum area)
    {
        musicEventInstance.setParameterByName("area", (int) area);
        Debug.Log(musicEventInstance.isValid());
    }

    public void SetSlowTime(float time)
    {
        musicEventInstance.setParameterByName("TimeSlow", time);
    }
    
    public void SetFootstepArea(GroundCheck.GroundType area)
    {
        footstepEventInstance.setParameterByName("Ground", (float)area);
    }

    public void SetFootstepRunning(int running)
    {
        footstepEventInstance.setParameterByName("Running", running);
    }
    
    public void StopEventInstance(EventInstance eventInstance)
    {
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }
    
    /*public void DisableSound()
    {
        psound = GetComponent<PlaySound>();
        EventInstance eventInstance = psound.PlaySoundEvent;
        instance.StopEventInstance(eventInstance);
    }*/
    
    public bool IsPlaying(EventInstance instance) {
        PLAYBACK_STATE state;   
        instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }
}
