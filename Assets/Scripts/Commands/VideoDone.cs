using UnityEngine;
using UnityEngine.Video;

public class VideoDone : MonoBehaviour
{
    VideoPlayer video_player;
    SceneLoader scene_laoder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        scene_laoder = GetComponent<SceneLoader>();
        video_player = GetComponent<VideoPlayer>();
        video_player.loopPointReached += WhenVideoEnds;
    }

    void WhenVideoEnds(VideoPlayer video_player){
        
        scene_laoder.TotalSceneLoading();
    }
}
