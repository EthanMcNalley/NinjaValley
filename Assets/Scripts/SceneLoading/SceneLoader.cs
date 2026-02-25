using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneField[] scenes_to_load;
    [SerializeField] private SceneField[] scenes_to_unload;
    public SceneField persistables_scene;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            LoadScenes();
            UnloadScenes();
        }
    }

    private void LoadScenes(){
        for (int i = 0; i < scenes_to_load.Length; i++){
            bool is_scene_loaded = false;

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loaded_scene = SceneManager.GetSceneAt(j);

                if (loaded_scene.name == scenes_to_load[i].SceneName)
                {
                    is_scene_loaded = true;
                    break;
                }
            }
            
            if (!is_scene_loaded)
            {
                StartCoroutine(Wait(i));
                //SceneManager.LoadSceneAsync(scenes_to_load[i], LoadSceneMode.Additive);
            }
        }
    }
    
    private IEnumerator Wait(int i)
    {
        Debug.Log(scenes_to_load[i]);
        //yield return SceneManager.LoadSceneAsync(scenes_to_load[i].SceneName, LoadSceneMode.Additive);
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scenes_to_load[i].SceneName, LoadSceneMode.Additive);
        
        asyncOp.allowSceneActivation = false;

        while (asyncOp.progress < 0.5f)
        {
            yield return null;
        }

        asyncOp.allowSceneActivation = true;
        
        while (!asyncOp.isDone)
        {
            yield return null;
        }
        
        /*while (!SceneManager.GetSceneByName(scenes_to_load[i].SceneName).isLoaded)
        {
            yield return null;
        }*/
    }

    private void UnloadScenes(){
        for (int i = 0; i < scenes_to_unload.Length; i++){
            for (int j = 0; j < SceneManager.sceneCount; j++){
                
                Scene loaded_scene = SceneManager.GetSceneAt(j);

                if (loaded_scene.name == scenes_to_unload[i].SceneName){
                    SceneManager.UnloadSceneAsync(scenes_to_unload[i].SceneName);
                }
            }
        }
    }

    public void TotalSceneLoading(){
        SceneManager.LoadScene(persistables_scene.SceneName);
        LoadScenes();
    }
}
