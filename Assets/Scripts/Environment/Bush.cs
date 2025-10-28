using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bush : MonoBehaviour
{
    Material mat;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        mat = GetComponent<Renderer>().material;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackHitBox"))
        {
            animator.SetTrigger("Damage");
            Destroy(gameObject, 1f);
        }
    }
}
