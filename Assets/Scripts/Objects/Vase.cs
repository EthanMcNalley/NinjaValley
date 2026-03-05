using Mono.Cecil;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Vase : MonoBehaviour
{
    Animator animator;
    public GameObject essence;
    public FMODUnity.EventReference breakSound;
    void Start()
    {
        if (TryGetComponent<Animator>(out animator))
        {
            animator = GetComponent<Animator>();
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
            
            if (essence != null) Instantiate(essence, transform.position, transform.rotation);
            AudioManager.instance.PlayOneShot(breakSound, transform.position);

            gameObject.SetActive(false);
        }
    }
}
