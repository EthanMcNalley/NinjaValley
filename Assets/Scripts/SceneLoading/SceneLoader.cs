using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneField[] scenes_to_load;
    [SerializeField] private SceneField[] scenes_to_unload;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

            for (int j = 0; j < SceneManager.sceneCount; j++){
                Scene loaded_scene = SceneManager.GetSceneAt(j);

                if (loaded_scene.name == scenes_to_load[i].SceneName){
                    is_scene_loaded = true;
                    break;
                }
            }

            if (!is_scene_loaded){
                SceneManager.LoadSceneAsync(scenes_to_load[i], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadScenes(){
        for (int i = 0; i < scenes_to_unload.Length; i++){
            for (int j = 0; j < SceneManager.sceneCount; j++){
                
                Scene loaded_scene = SceneManager.GetSceneAt(j);

                if (loaded_scene.name == scenes_to_unload[i].SceneName){
                    SceneManager.UnloadSceneAsync(scenes_to_unload[i]);
                }
            }
        }
    }
}
