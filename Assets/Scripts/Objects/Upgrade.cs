using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public enum UpgradeType
    {
        TIMESLOW,
        DOUBLEJUMP,
        HEALTH,
        TIMEAMOUNT,
        SHADOW
    }

    public UpgradeType upgrade_type;
}
