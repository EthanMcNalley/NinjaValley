using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeLayers : MonoBehaviour
{
    public UniversalRendererData renderer_data;
    public RenderObjects render_objects;
    public LayerMask default_layers;
    public LayerMask time_slow_layers;

    public void ChangeToDefault()
    {
        render_objects.settings.filterSettings.LayerMask = default_layers;
    }

    public void ChangeToTimeSlow()
    {
        render_objects.settings.filterSettings.LayerMask = time_slow_layers;
    }
}
