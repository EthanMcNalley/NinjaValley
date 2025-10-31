using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFirstSelect : MonoBehaviour
{
    [SerializeField] private Selectable first;

    
    
    void OnEnable()
    {
        // Clear then set to ensure selection sticks even after mouse use
        EventSystem.current.SetSelectedGameObject(null);
        if (first)
        {
            EventSystem.current.SetSelectedGameObject(first.gameObject);
        }
    }
}