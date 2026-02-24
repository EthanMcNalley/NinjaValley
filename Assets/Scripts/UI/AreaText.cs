using UnityEngine;

public class AreaText : MonoBehaviour
{
    public static bool active = false;
    public GameObject text;

    // Update is called once per frame
    void Update()
    {
        text.SetActive(active);
    }
}
