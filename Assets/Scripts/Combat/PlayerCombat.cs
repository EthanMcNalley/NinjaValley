using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private GameObject player;

    private float sinceLastCombo;
    private float timeBetweenCombos = 1.5f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
