using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public float healthPoint = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
        healthPoint -= damage;
        Debug.Log(healthPoint);
    }
}
