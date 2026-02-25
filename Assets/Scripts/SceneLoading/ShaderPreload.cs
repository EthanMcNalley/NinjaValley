using UnityEngine;

public class ShaderPrewarm : MonoBehaviour
{
    [SerializeField] private ShaderVariantCollection collection;

    void Awake()
    {
        if (collection != null)
        {
            collection.WarmUp();
        }
    }
}
