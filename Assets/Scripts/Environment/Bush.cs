using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bush : MonoBehaviour
{
    Material mat;
    Animator animator;
    void Start()
    {
        if (TryGetComponent<Animator>(out animator))
        {
            animator = GetComponent<Animator>();
        }

        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            mat = GetComponent<Renderer>().material;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackHitBox"))
        {
            if (animator != null)
            {
                animator.SetTrigger("Damage");
            }

            Destroy(gameObject, 1f);
        }
    }
}
