using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class TitleScreenStuff : MonoBehaviour
{
    public EventReference PlaySoundClip;
    public EventInstance PlaySoundEvent;
    public GameObject river;
    
    public void PlayOneShot(string path)
    {
        RuntimeManager.PlayOneShot(path);
    }

    public void SetActiveTrue()
    {
        river.SetActive(true);
    }

    public void StopAudio()
    {
        PlaySoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
