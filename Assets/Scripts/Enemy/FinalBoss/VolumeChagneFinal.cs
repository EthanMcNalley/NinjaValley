using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeChagneFinal : MonoBehaviour
{
    public Volume volume;
    [SerializeField] private float transitionTime = 0.4f;

    void OnEnable()
    {
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
