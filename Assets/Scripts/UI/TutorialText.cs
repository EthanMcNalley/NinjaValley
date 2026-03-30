using System.Collections;
using EasyTextEffects;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public TextMeshPro tutorial_text;

    void Start()
    {
        //tutorial_text.enabled = false;
        Invisible();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Visible());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorial_text.GetComponent<TextEffect>().StartManualEffect("fadeout");
        }
    }

    public void Invisible()
    {
        tutorial_text.color = Color.clear;
    }

    IEnumerator Visible()
    {
        tutorial_text.GetComponent<TextEffect>().StartManualEffect("fadein");

        yield return new WaitForEndOfFrame();

        tutorial_text.color = Color.white;
    }
    
}
