using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : EnemyHPUI
{
    [SerializeField] private Slider breakBar;

    public void UpdateBreakBar(float currBreak, float maxBreak)
    {
        ratio = Mathf.Clamp01(currBreak / maxBreak);
        breakBar.value = ratio;
    }
}