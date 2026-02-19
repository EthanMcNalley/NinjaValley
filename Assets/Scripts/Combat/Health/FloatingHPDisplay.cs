using UnityEngine;
using UnityEngine.UI;

public class FloatingHPDisplay : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider shadowBar;
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
    
    public void UpdateHealthBar(float currHealth, float maxHealth, float shadowDamage)
    {
        ratio = Mathf.Clamp01((currHealth - shadowDamage) / maxHealth);
        healthBar.value = ratio;
        
        ratio = Mathf.Clamp01(currHealth / maxHealth);
        shadowBar.value = ratio;
        
        adjustedRatio = Mathf.Pow(ratio, 1.3f);
        healthFillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);
    }

    public void UpdateShadowBar(float currHealth, float maxHealth)
    {
        ratio = Mathf.Clamp01(currHealth / maxHealth);
        shadowBar.value = ratio;
    }
    
    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
