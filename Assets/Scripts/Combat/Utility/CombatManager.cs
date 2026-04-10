using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager instance { get; private set; }
    
    [SerializeField]private int enemiesInCombat = 0;
    [SerializeField]private bool playerInCombat;
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
            playerInCombat = true;
        }
    }

    public void RemoveEnemyFromCombat()
    {
        enemiesInCombat--;

        if (enemiesInCombat <= 0)
        {
            enemiesInCombat = 0;
            CombatEvents.RaisePlayerInCombatEnded();
            playerInCombat = false;
        }
    }

    public void ResetBoss()
    {
        var boss = GameObject.FindGameObjectWithTag("Temp");

        if (boss != null)
        {
            if (boss.transform.Find("AnsonTenguBoss").name == "AnsonTenguBoss")
            {
                boss.transform.Find("AnsonTenguBoss").gameObject.SetActive(false);
            }

            boss = GameObject.FindGameObjectWithTag("FoxGirl");
            Debug.Log("TE");
            if (boss != null)
            {
                Debug.Log("TEST");
                boss.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
