using UnityEngine;

public class LockedBars : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OpenBars(float delay)
    {
        Invoke("OpenBar", delay);
    }
    public void CloseBars(float delay)
    {
        Invoke("CloseBar", delay);
    }

    void OpenBar()
    {
        animator.SetBool("Active", true);
    }

    void CloseBar()
    {
        animator.SetBool("Active", false);
    }
}
