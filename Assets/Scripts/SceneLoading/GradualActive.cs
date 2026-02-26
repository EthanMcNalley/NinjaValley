using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualActive : MonoBehaviour
{
    public List<GameObject> gameObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        
        foreach (var gameobject in gameObjects)
        {
            gameobject.SetActive(true);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
