using UnityEngine;

public class InteractTextBox : MonoBehaviour
{
    private UIManager UI_manager;
    private bool inside = false;
    public string text = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI_manager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inside){
            if (Input.GetKeyDown(KeyCode.Q)){
                if (UIManager.ui_state == UIManager.UIState.INACTIVE){
                    UI_manager.OpenTextScrollMenu(text);
                }

                else{
                    UI_manager.CloseTextScrollMenu();
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Player")){
            inside = true;
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.CompareTag("Player")){
            inside = false;
        }
    }
}
