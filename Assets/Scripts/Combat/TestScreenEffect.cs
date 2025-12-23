using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TestScreenEffect : MonoBehaviour
{
    public float time_size;
    public float material_rate = 3.0f;
    private bool expanding = true;
    public Material time_slow_material;
    private Material instance_material;
    public FullScreenPassRendererFeature time_slow_renderer;

    void Start()
    {
        instance_material = new Material(time_slow_material);
        time_slow_renderer.passMaterial = instance_material;
    }
    void Update()
    {
        if (expanding) {
            time_size += Time.deltaTime * material_rate;
            if (time_size >= 3f) expanding = false;
        } else {
            time_size -= Time.deltaTime * material_rate;
            if (time_size <= 0f) expanding = true;
        }
        
        instance_material.SetFloat("_WipeSize", time_size);
    }
}
