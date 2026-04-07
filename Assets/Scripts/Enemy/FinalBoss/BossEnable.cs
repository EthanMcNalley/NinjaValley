using System;
using UnityEngine;

public class BossEnable : MonoBehaviour
{
    public GameObject boss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !boss.activeSelf)
        {
            boss.SetActive(true);
        }
    }
}
