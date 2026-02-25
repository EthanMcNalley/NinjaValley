using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneField[] scenes_to_load;
    [SerializeField] private SceneField[] scenes_to_unload;
    [SerializeField] private SceneField[] scenes_to_skip_load;
    public SceneField persistables_scene;

    private HashSet<String> skipScenes;
    
    void Awake()
    {
        skipScenes = new HashSet<string>();
        foreach (SceneField scene in scenes_to_skip_load)
        {
            skipScenes.Add(scene.SceneName);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            Vector3 direction = (other.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, direction);

            if (dot > 0)
            {
                UnloadScenes(scenes_to_unload);
                LoadScenes(scenes_to_load);
            }
            else
            {
                UnloadScenes(scenes_to_load);
                LoadScenes(scenes_to_unload);
            }
        }
    }

    private void LoadScenes(SceneField[] scenes){
        for (int i = 0; i < scenes.Length; i++){
            bool is_scene_loaded = false;
            
            if (skipScenes.Contains(scenes[i].SceneName)) continue;

            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loaded_scene = SceneManager.GetSceneAt(j);

                if (loaded_scene.name == scenes[i].SceneName)
                {
                    is_scene_loaded = true;
                    break;
                }
            }
            
            if (!is_scene_loaded)
            {
                StartCoroutine(Wait(i, scenes));
                //SceneManager.LoadSceneAsync(scenes_to_load[i], LoadSceneMode.Additive);
            }
        }
    }
    
    private IEnumerator Wait(int i, SceneField[] scenes)
    {
        Debug.Log(scenes[i]);
        //yield return SceneManager.LoadSceneAsync(scenes_to_load[i].SceneName, LoadSceneMode.Additive);
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scenes[i].SceneName, LoadSceneMode.Additive);
        
        asyncOp.allowSceneActivation = false;

        while (asyncOp.progress < 0.9f)
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

    private void UnloadScenes(SceneField[] scenes){
        for (int i = 0; i < scenes.Length; i++){
            for (int j = 0; j < SceneManager.sceneCount; j++){
                
                Scene loaded_scene = SceneManager.GetSceneAt(j);

                if (loaded_scene.name == scenes[i].SceneName){
                    SceneManager.UnloadSceneAsync(scenes[i].SceneName);
                }
            }
        }
    }

    public void TotalSceneLoading(){
        SceneManager.LoadScene(persistables_scene.SceneName);
        LoadScenes(scenes_to_load);
    }
}
