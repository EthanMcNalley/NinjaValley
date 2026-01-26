using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShaderVariables : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
        Shader.SetGlobalFloat("_UnscaledDeltaTime", Time.unscaledDeltaTime);
    }
}
