using UnityEngine;

public class AttackandDodge : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    public float dodgeCooldown, attackCooldown;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(animator != null)
            animator.SetTrigger("attack");
        }

    }

    
}
