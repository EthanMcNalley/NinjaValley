using UnityEngine;
using System;
using Unity.Cinemachine;
using FMODUnity;

public class PortalFinal : MonoBehaviour
{
    public Transform player;
    public Transform otherPortal;
    public float offset = 5f;

    private bool playerIsOverLapping;
    public Color portalColor;
    public Quaternion rotationDifference = Quaternion.Euler(0f, 180f, 0f);
    
    public static event Action<PortalFinal> OnPlayerTeleported;
    private CinemachineBrain cinemachineBrain;
    private NewMovement movement;
    public EventReference enterSound;
    
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        movement = player.GetComponent<NewMovement>();
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
                AudioManager.instance.PlayOneShot(enterSound, transform.position);
                
                Quaternion rotationDiff = otherPortal.rotation * Quaternion.Inverse(transform.rotation);
                rotationDiff *= rotationDifference; //maybe not needed
                
                player.rotation = rotationDiff * player.rotation;
                
                Vector3 positionOffset = rotationDiff * portalToPlayer;
                
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
