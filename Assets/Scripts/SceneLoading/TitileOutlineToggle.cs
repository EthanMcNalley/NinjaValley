using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class TitileOutlineToggle : MonoBehaviour
{
    public FullScreenPassRendererFeature outlineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.activeSceneChanged += SceneManagerActiveSceneChanged;
        outlineRenderer.SetActive(false);
    }

    void SceneManagerActiveSceneChanged(Scene from, Scene to)
    {
        outlineRenderer.SetActive(true);   
    }
}
