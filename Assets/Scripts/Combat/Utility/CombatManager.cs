using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager instance { get; private set; }
    
    [SerializeField]private int enemiesInCombat = 0;
    //This whole thing is to manage if the player is in combat or not
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    //Enemies will call these functions
    public void AddEnemyToCombat()
    {
        enemiesInCombat++;

        if (enemiesInCombat == 1)
        {
            CombatEvents.RaisePlayerInCombat();
        }
    }

    public void RemoveEnemyFromCombat()
    {
        enemiesInCombat--;

        if (enemiesInCombat <= 0)
        {
            enemiesInCombat = 0;
            CombatEvents.RaisePlayerInCombatEnded();
        }
    }
}
