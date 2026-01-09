using UnityEngine;
using UnityEngine.UI;

public class AlphaCheck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
    }
}
