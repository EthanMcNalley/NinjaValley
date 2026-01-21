using EasyTextEffects;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(":)");
            GetComponent<TextMeshPro>().enabled = true;
            GetComponent<TextEffect>().StartManualEffect("fadein");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(":(");
            GetComponent<TextEffect>().StartManualEffect("fadeout");
        }
    }
}
