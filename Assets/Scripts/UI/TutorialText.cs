using EasyTextEffects;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public TextMeshPro tutorial_text;

    void Start()
    {
        //tutorial_text.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(":)");
            //tutorial_text.enabled = true;
            tutorial_text.color = Color.white;
            tutorial_text.GetComponent<TextEffect>().StartManualEffect("fadein");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(":(");
            tutorial_text.GetComponent<TextEffect>().StartManualEffect("fadeout");
        }
    }

    public void Invisible()
    {
        tutorial_text.color = Color.clear;
    }

    public void Visible()
    {
        tutorial_text.color = Color.white;
    }
    
}
