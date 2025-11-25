using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    public void QuitTheGame(){
        Application.Quit();
    }

    public void ToTitleScreen(){
        SceneManager.LoadScene("TitleScene");
    }
}
