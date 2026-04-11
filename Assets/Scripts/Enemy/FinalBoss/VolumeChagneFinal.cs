using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VolumeChagneFinal : MonoBehaviour
{
    public Volume volume;
    [SerializeField] private float transitionTime = 0.4f;

    void OnEnable()
    {
        Scene persistables = SceneManager.GetSceneByName("RealPersistables");
        foreach (GameObject go in persistables.GetRootGameObjects())
        {
            if (go.name == "FinalBossTransitionVolume(Clone)")
            {
                volume = go.GetComponent<Volume>();
                break;
            }
        }
        StartCoroutine(ChangeVolumeCoroutine());
    }

    IEnumerator ChangeVolumeCoroutine()
    {
        while (volume.weight > 0)
        {
            volume.weight = Mathf.MoveTowards(volume.weight, 0, Time.deltaTime * transitionTime);
            yield return null;
        }
    }
}
