using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOneScene : MonoBehaviour
{
    public SceneField load_this_scene;
    public void LoadThisOneScene()
    {
        SceneManager.LoadScene(load_this_scene);
    }
}
