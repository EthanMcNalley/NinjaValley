using UnityEngine;

public class LockedBars : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OpenBars()
    {
        animator.SetBool("Active", true);
    }
    public void CloseBars()
    {
        animator.SetBool("Active", false);
    }
}
