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
        
        //vca.getVolume(out volume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        SetVolume(volume);
        volumeSlider.value = volume;
    }

    public void SetVolume(float value)
    {
        if (value == 0)
        {
            vca.setVolume(0);
            return;
        }
        
        float minDb = -30f;
        float maxDb = 0f;
        
        float db = Mathf.Lerp(minDb, maxDb, value);
        float final = Mathf.Pow(10f, db/20f);
        
        vca.setVolume(final);
    }
}
