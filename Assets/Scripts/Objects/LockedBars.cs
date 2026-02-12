using UnityEngine;

public class LockedBars : MonoBehaviour
{
    public bool starting_closed = false;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Active", starting_closed);
    }
    
    public void OpenBars(float delay)
    {
        Invoke("OpenBar", delay);
    }
    public void CloseBars(float delay)
    {
        Debug.Log("Almost");
        Invoke("CloseBar", delay);
    }

    void OpenBar()
    {
        animator.SetBool("Active", false);
    }

    void CloseBar()
    {
        animator.SetBool("Active", true);
    }
}
