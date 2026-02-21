using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShaderVariables : MonoBehaviour
{
    float dissolve_time = 0;
    // Update is called once per frame
    void Update()
    {
        if (TimeManager.time_state == TimeManager.TimeState.NORMAL)
        {
            if (dissolve_time > 0){
                dissolve_time -= Time.deltaTime;
            }
        }

        else
        {
            if (dissolve_time < 1){
                dissolve_time += Time.deltaTime;
            }
        }

        Shader.SetGlobalFloat("_DissolveAmount", dissolve_time);
        Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
        Shader.SetGlobalFloat("_UnscaledDeltaTime", Time.unscaledDeltaTime);
        Shader.SetGlobalFloat("_TimeSpeed", TimeManager.slowed_amount);
    }
}
