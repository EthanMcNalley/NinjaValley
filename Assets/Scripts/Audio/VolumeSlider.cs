using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class VolumeSlider : MonoBehaviour
{
    public string vcaPath;
    public FMOD.Studio.VCA vca;
    public Slider volumeSlider;
    
    public float volume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vca = RuntimeManager.GetVCA(vcaPath);
        
        vca.getVolume(out volume);
        volumeSlider.value = volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        float minDb = -30f;
        float maxDb = 0f;
        
        float db = Mathf.Lerp(minDb, maxDb, value);
        float final = Mathf.Pow(10f, db/20f);
        
        vca.setVolume(final);
    }
}
