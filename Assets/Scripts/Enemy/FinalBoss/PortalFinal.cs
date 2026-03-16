using UnityEngine;
using System;

public class PortalFinal : MonoBehaviour
{
    public Transform player;
    public Transform otherPortal;
    public float offset = 5f;

    private bool playerIsOverLapping;
    
    public static event Action<PortalFinal> OnPlayerTeleported;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (playerIsOverLapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer); //change lhs if dont teleport...
            
            Debug.Log(dotProduct);

            if (dotProduct < 0f)
            {
                OnPlayerTeleported?.Invoke(this);
                float rotationDiff = -Quaternion.Angle(transform.rotation, otherPortal.rotation);
                rotationDiff += 180f; //maybe not needed
                player.Rotate(Vector3.up, rotationDiff);
                
                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = otherPortal.position + positionOffset - otherPortal.forward * offset;
                
                playerIsOverLapping = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverLapping = true;
        }
    }
}
