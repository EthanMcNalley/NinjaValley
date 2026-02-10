using UnityEngine;

public class SetAnim : MonoBehaviour
{
    float delay;
    public Animator animator;
    string trigger_string;
    public void SetAnimationTrigger(string trigger){
        trigger_string = trigger;
        Invoke("PlayAnimation", delay);
    }

    void PlayAnimation()
    {
        animator.SetTrigger(trigger_string);
    }
}
