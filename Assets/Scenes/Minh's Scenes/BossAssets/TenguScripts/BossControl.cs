using UnityEngine;

public class BossControl : MonoBehaviour
{
    Animator animator;
    GameObject TornadoVFX, RockSpikeVFX;
    GameObject spearHitbox, airAttackHitBox;
    Vector3 playerPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void SpawnTornadoVFX()
    {
        TornadoVFX = Instantiate(Resources.Load<GameObject>("Prefabs/TornadoVFX"), transform.position, Quaternion.identity);
        
    }

    public void SpawnRockSpikeVFX()
    {
        RockSpikeVFX = Instantiate(Resources.Load<GameObject>("Prefabs/RockSpikeVFX"), transform.position, Quaternion.identity);
    }

    public void SpawnAirAttackHitBox() 
    {
        airAttackHitBox = Instantiate(Resources.Load<GameObject>("Prefabs/AirAttackHitBox"), transform.position, Quaternion.identity);
    }
}
