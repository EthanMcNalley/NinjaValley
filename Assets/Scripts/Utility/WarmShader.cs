using UnityEngine;

public class WarmShader : MonoBehaviour
{
    public ShaderVariantCollection mShaderVariantCollection;

    void Awake()
    {
        if (mShaderVariantCollection != null && !mShaderVariantCollection.isWarmedUp)
        {
            mShaderVariantCollection.WarmUp();
        }
    }
}
