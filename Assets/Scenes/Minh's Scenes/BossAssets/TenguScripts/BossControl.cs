using UnityEngine;

public class BossControl : MonoBehaviour
{
    Animator animator;
    GameObject TornadoVFX, RockSpikeVFX;
    GameObject speakHitbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
