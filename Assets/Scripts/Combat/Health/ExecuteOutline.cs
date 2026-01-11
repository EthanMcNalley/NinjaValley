using UnityEngine;
using UnityEngine.UI;
public class ExecuteOutline : MonoBehaviour
{
    public Image outline;
    public float speed = 5f;

    private bool fill;
    private Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Image>();
        color = outline.color;
    }

    void OnEnable()
    {
        fill = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fill && outline.fillAmount <= 1f)
        {
            outline.fillAmount = Mathf.MoveTowards(outline.fillAmount, 1f, speed * Time.deltaTime);
        }

        if (outline.fillAmount >= 1f)
        {
            color.a = Mathf.PingPong(Time.time * 1.5f, 1f);
            outline.color = color;
        }
    }
}
