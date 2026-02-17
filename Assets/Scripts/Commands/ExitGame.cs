using UnityEngine;
using UnityEditor;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
        
        #if  UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}
