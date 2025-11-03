using UnityEngine;
using UnityEngine.UI;

public class FloatingHPDisplay : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private Image fillImage;
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
        fillImage = healthBar.fillRect.GetComponent<Image>();
        fillImage.color = maxColor;
    }
    
    public void UpdateHealthBar(float currHealth, float maxHealth)
    {
        ratio = Mathf.Clamp01(currHealth / maxHealth);
        healthBar.value = ratio;
        
        adjustedRatio = Mathf.Pow(ratio, 1.3f);
        fillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
