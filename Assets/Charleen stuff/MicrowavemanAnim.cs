using UnityEngine;
using UnityEngine.InputSystem;


public class MicrowavemanAnim : MonoBehaviour
{
    public Animator microwaveAnim;

    private bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.actions.FindAction("Jump").triggered)
        {
            microwaveAnim.SetTrigger("isJump");
        }

        if (isGrounded)
        {
            if (InputSystem.actions.FindAction("Move").IsPressed())
            {
                microwaveAnim.SetBool("isWalk", true);
            }

            else
            {
                microwaveAnim.SetBool("isWalk", false);
            }
        }

        if (!isGrounded)
        {
            microwaveAnim.SetBool("isWalk", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Kurt")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Kurt")
        {
            isGrounded = false;
        }
    }
}
