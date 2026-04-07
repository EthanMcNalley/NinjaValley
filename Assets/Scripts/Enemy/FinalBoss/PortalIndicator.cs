using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PortalIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab; // a simple UI image prefab
    [SerializeField] private Transform indicatorContainer; // horizontal/vertical layout group
    
    [SerializeField] private Sprite[] portalSprites;
    [SerializeField] private Material[] portalMaterials;
    [SerializeField] private List<GameObject> activeIndicators = new List<GameObject>();
    private Dictionary<GameObject, Color> indicatorColors = new Dictionary<GameObject, Color>();
    public bool cleared = false;

    public void SetRequiredPortals(List<Color> portalColors, List<Color> allColors)
    {
        cleared = false;
        foreach (var indicator in activeIndicators)
        {
            Destroy(indicator);
        }

        activeIndicators.Clear();
        
        foreach (Color color in portalColors)
        {
            /*GameObject indicator = Instantiate(indicatorPrefab, indicatorContainer);
            //indicator.GetComponent<Image>().color = color;
            activeIndicators.Add(indicator);*/
            
            GameObject indicator = Instantiate(indicatorPrefab, indicatorContainer);
            Image img = indicator.GetComponent<Image>();
    
            int index = allColors.IndexOf(color);
            if (index >= 0 && index < portalSprites.Length)
            {
                img.sprite = portalSprites[index];
                img.material = portalMaterials[index];
                img.useSpriteMesh = true;
            }
            
            img.color = Color.white;
            indicatorColors[indicator] = color;
    
            activeIndicators.Add(indicator);
        }
    }

    /*public void MarkPortalComplete(Color portalColor)
    {
        if (activeIndicators.Count == 0) return;
        
        bool destroyed = false;
        
        //remove the completed portal indicator
        foreach (var indicator in activeIndicators)
        {
            Image img = indicator.GetComponent<Image>();
            if (img.color == portalColor)
            {
                activeIndicators.Remove(indicator);
                Destroy(indicator);
                destroyed = true;
                break;
            }
        }

        if (activeIndicators.Count == 0 && destroyed)
        {
            cleared = true;
        }
    }*/
    
    public void MarkPortalComplete(Color portalColor)
    {
        if (activeIndicators.Count == 0) return;

        bool destroyed = false;

        foreach (var indicator in activeIndicators)
        {
            if (indicatorColors.TryGetValue(indicator, out Color c) && c == portalColor)
            {
                indicatorColors.Remove(indicator);
                activeIndicators.Remove(indicator);
                Destroy(indicator);
                destroyed = true;
                break;
            }
        }

        if (activeIndicators.Count == 0 && destroyed)
            cleared = true;
    }

    public void ClearPortals()
    {
        foreach (var indicator in activeIndicators)
        {
            Destroy(indicator);
        }
        indicatorColors.Clear();
        activeIndicators.Clear();
    }
}

[System.Serializable]
public struct PortalSpriteEntry
{
    public Color color;
    public Sprite sprite;
}
