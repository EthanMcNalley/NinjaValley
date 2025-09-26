using UnityEngine;
using UnityEngine.InputSystem;


public class MicrowavemanAnim : MonoBehaviour
{
    public Animator microwaveAnim;

    private bool isGrounded;

    // Update is called once per frame
    void Update()
    {
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

            microwaveAnim.SetBool("isJump", false);
        }

        else
        {
            microwaveAnim.SetBool("isJump", true);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Kurt")
        {
            isGrounded = true;
        }
    }
}
