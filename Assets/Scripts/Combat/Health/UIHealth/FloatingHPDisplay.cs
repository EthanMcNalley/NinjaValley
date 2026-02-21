using UnityEngine;
using UnityEngine.UI;

public class FloatingHPDisplay : EnemyHPUI
{
    private Camera cam;
    public Transform target;
    public Vector3 offset;
    
    public Color maxColor = Color.green;
    public Color minColor = Color.red;
    
    private float adjustedRatio;

    void Start()
    {
        cam = Camera.main;
        healthFillImage.color = maxColor;
    }
    
    public override void UpdateHealthBar(float currHealth, float maxHealth, float shadowDamage)
    {
        base.UpdateHealthBar(currHealth, maxHealth, shadowDamage);
        
        adjustedRatio = Mathf.Pow(ratio, 1.3f);
        healthFillImage.color = Color.Lerp(minColor, maxColor, adjustedRatio);
    }
    
    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
