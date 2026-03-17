using UnityEngine;
using FMODUnity;
using UnityEditor.ShaderGraph.Internal;

public class Checkpoint : MonoBehaviour
{
    public Material inactive_material;
    public Material active_material;
    Animator animator;
    public EventReference bell_sound;
    private UIManager UI_manager;
    public static bool first_checkpoint = true;
    void Start()
    {
        UI_manager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
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
                animator.SetTrigger("BellSwing");
            }

            if (first_checkpoint)
            {
                    UI_manager.OpenTextScrollMenu("Checkpoint Bell Activated! You will respawn here upon death.");
            }

            first_checkpoint = false;

            GetComponent<MeshRenderer>().material = active_material;
            
            AudioManager.instance.PlayOneShot(bell_sound, transform.position);
        }
    }
}
