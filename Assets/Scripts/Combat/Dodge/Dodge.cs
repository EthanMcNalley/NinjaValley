using UnityEngine;
using UnityEngine.InputSystem;

public class Dodge : MonoBehaviour
{
    private InputAction dodgeAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dodgeAction = InputSystem.actions.FindAction("Dodge");
    }

    // Update is called once per frame
    void Update()
    {
        if (dodgeAction.triggered)
        {
            Debug.Log("Dodge");
        }
    }

    void OnEnable()
    {
        dodgeAction.Enable();
    }

    void OnDisable()
    {
        dodgeAction.Disable();
    }
}
