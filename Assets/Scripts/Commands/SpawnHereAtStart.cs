using UnityEngine;
using System.Collections;

public class SpawnHereAtStart : MonoBehaviour
{
    Transform spawn_point;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return null;
        
        spawn_point = GetComponent<Transform>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NewMovement.revive_position = spawn_point.position;
        NewMovement.time_able = false;
        player.GetComponent<NewMovement>().double_jump_able = false;
        player.transform.position = spawn_point.position;
        player.transform.rotation = Quaternion.identity;
        AudioManager.instance.SetMusicArea(MusicEnum.Tutorial);
    }
}
