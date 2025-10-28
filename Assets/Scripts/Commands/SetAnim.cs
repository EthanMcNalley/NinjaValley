using UnityEngine;

public class SetAnim : MonoBehaviour
{
    public Animator animator;
    public void SetAnimationTrigger(string trigger){
        animator.SetTrigger(trigger);
    }
}
