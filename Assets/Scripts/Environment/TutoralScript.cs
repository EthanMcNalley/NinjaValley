using UnityEngine;

public class TutoralScript : MonoBehaviour
{
    public GameObject part2Text, part3Text;
    public GameObject umbrella1, umbrella2, lantern;
    private bool part2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!umbrella2.activeInHierarchy && !part2)
        {
            lantern.SetActive(true);
            part3Text.SetActive(true);
            part2 =  true;
        }
    }
}
