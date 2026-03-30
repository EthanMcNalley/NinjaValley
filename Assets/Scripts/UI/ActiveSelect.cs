using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActiveSelect : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectButton;

    void OnEnable()
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(selectButton);
    }
}
