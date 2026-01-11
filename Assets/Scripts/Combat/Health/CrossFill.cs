using UnityEngine;
using UnityEngine.UI;
public class CrossFill : MonoBehaviour
{
    public Image cross;
    public float speed = 1f;

    private bool fill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cross = GetComponent<Image>();
    }

    void OnEnable()
    {
        fill = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fill && cross.fillAmount <= 1f)
        {
            cross.fillAmount = Mathf.MoveTowards(cross.fillAmount, 1f, speed * Time.deltaTime);
        }
    }
}
