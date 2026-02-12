using UnityEngine;

public class DeactivateAfterTime : MonoBehaviour
{
    public float time_alive;

    void OnEnable()
    {
        Invoke("Deactivate", time_alive);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
