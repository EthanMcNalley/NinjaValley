using UnityEngine;
using FMODUnity;
public class Vase : MonoBehaviour
{
    Animator animator;
    public GameObject health_essence;
    public GameObject shadow_essence;
    public EventReference breakSound;
    public GameObject break_particles;
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
            
            int randnum = Random.Range(0, 2);
            
            if (randnum == 0)
            {
                if (health_essence != null){
                    Instantiate(health_essence, transform.position, transform.rotation);
                    AudioManager.instance.PlayOneShot(breakSound, transform.position);
                }
            }

            else
            {
                if (shadow_essence != null){
                    Instantiate(shadow_essence, transform.position, transform.rotation);
                    AudioManager.instance.PlayOneShot(breakSound, transform.position);
                }
            }

            Instantiate(break_particles, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
