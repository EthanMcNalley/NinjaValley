using UnityEngine;
using UnityEngine.UI;

public class FloatingHPDisplay : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private Camera cam;
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        cam = Camera.main;
    }
    
    public void UpdateHealthBar(float currHealth, float maxHealth)
    {
        healthBar.value = Mathf.Clamp01(currHealth / maxHealth);
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
