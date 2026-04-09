using UnityEngine;

public class NoDampCam : MonoBehaviour
{
    public Vector3 original_damp_amount = new Vector3(1.0f, 1.0f, 1.7f);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControlling.damping_amount = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraControlling.damping_amount = original_damp_amount;
        }
    }
}
