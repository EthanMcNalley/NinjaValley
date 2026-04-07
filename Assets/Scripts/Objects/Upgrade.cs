using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public GameObject spawn_object;
    public enum UpgradeType
    {
        TIMESLOW,
        DOUBLEJUMP,
        HEALTH,
        TIMEAMOUNT,
        SHADOW
    }

    public UpgradeType upgrade_type;

    public void SpawnOnCollect()
    {
        if (spawn_object != null)
        {
            spawn_object.SetActive(true);
        }
    }
}


