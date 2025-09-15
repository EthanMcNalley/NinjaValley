using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Adjust();
    }

    private void Adjust()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        
        float targetAspectRatio = 16.0f / 9.0f;
        
        float scaleHight = aspectRatio / targetAspectRatio;
        
        Camera camera = GetComponent<Camera>();

        if (scaleHight < 1.0f)
        {
            Rect rect = camera.rect;
            
            rect.width = 1.0f;
            rect.height = scaleHight;
            rect.x = 0.0f;
            rect.y = (1.0f - scaleHight) / 2.0f;
            
            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHight;
            
            Rect rect = camera.rect;
            
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0.0f;
            
            camera.rect = rect;
        }
    }
}
