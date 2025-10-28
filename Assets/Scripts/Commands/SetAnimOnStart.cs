using UnityEngine;

public class SetAnimOnStart : MonoBehaviour
{
    public string trigger;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(trigger);
    }

}
