using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody player;

    private Vector3 moveInput;

    public int speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.AddForce(-speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            player.AddForce(speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.AddForce(0, 0, -speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            player.AddForce(0, 0, speed);
        }
    }
}
