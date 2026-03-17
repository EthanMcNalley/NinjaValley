using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PortalIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab; // a simple UI image prefab
    [SerializeField] private Transform indicatorContainer; // horizontal/vertical layout group

    [SerializeField] private List<GameObject> activeIndicators = new List<GameObject>();
    public bool cleared = false;

    public void SetRequiredPortals(List<Color> portalColors)
    {
        cleared = false;
        foreach (var indicator in activeIndicators)
        {
            Destroy(indicator);
        }

        activeIndicators.Clear();
        
        foreach (Color color in portalColors)
        {
            GameObject indicator = Instantiate(indicatorPrefab, indicatorContainer);
            indicator.GetComponent<Image>().color = color;
            activeIndicators.Add(indicator);
        }
    }

    public void MarkPortalComplete(Color portalColor)
    {
        if (activeIndicators.Count == 0) return;
        
        bool destroyed = false;
        
        // Grey out or remove the completed portal indicator
        foreach (var indicator in activeIndicators)
        {
            Image img = indicator.GetComponent<Image>();
            if (img.color == portalColor)
            {
                Destroy(indicator);
                destroyed = true;
                break;
            }
        }

        if (activeIndicators.Count > 0 && destroyed)
        {
            cleared = true;
        }
    }

    public void ClearPortals()
    {
        foreach (var indicator in activeIndicators)
        {
            Destroy(indicator);
        }
        
        activeIndicators.Clear();
    }
}
