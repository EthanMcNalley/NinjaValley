using UnityEngine;
using UnityEngine.UI;

public class FloatingHPDisplay : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private Image healthFillImage;
    public Color maxColor = Color.green;
    public Color minColor = Color.red;
    private Camera cam;
    public Transform target;
    public Vector3 offset;
    
    private float ratio;
    private float adjustedRatio;

    void Start()
    {
        cam = Camera.main;
        healthFillImage = healthBar.fillRect.GetComponent<Image>();
        healthFillImage.color = maxColor;
    }
    
    public void UpdateHealthBar(float currHealth, float maxHealth)
    {
        ratio = Mathf.Clamp01(currHealth / maxHealth);
        healthBar.value = ratio;
        
        adjustedRatio = Mathf.Pow(ratio, 1.3f);
        healthFillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);
    }

    public void UpdateShadowBar(float currHealth, float maxHealth)
    {
        
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
