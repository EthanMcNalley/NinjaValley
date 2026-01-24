using UnityEngine;
using UnityEngine.UI;

public class KeepChildInPlace : MonoBehaviour
{
    Vector3 world_pos;
    public bool UI = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (UI){
            world_pos = GetComponent<RectTransform>().position;
        }

        else
        {
            world_pos = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UI)
        {
            GetComponent<RectTransform>().position = world_pos;
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            transform.position = world_pos;
        }
    }
}
