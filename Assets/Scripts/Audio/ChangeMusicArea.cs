using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicArea : MonoBehaviour
{
    public MusicEnum areaFrontZplus;
    public MusicEnum areaBackZminus;
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Player"))
        {
            Vector3 direction = (collider.transform.position - transform.position).normalized;
            
            float dot = Vector3.Dot(transform.forward, direction);

            AudioManager.instance.SetMusicArea(dot < 0 ? areaFrontZplus : areaBackZminus);
        }
    }
}    