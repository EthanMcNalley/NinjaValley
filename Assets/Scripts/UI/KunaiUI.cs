using UnityEngine;
using UnityEngine.UI;

public class KunaiUI : MonoBehaviour
{
    [SerializeField] Image kunaiUI1, kunaiUI2, kunaiUI3;
    public GameObject player;
    public CombatStateManager combatStateManager;
    private int prevKunai;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatStateManager = player.GetComponent<CombatStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combatStateManager.currentKunai == combatStateManager.maxKunai)
        {
            prevKunai = 3;
            return;
        }

        switch (combatStateManager.currentKunai)
        {
            case 0 :
                kunaiUI1.fillAmount = (combatStateManager.kunaiTimer / combatStateManager.kunaiChargeCooldown);
                if (prevKunai == 1)
                {
                    kunaiUI2.fillAmount = 0f;
                    kunaiUI3.fillAmount = 0f;
                }
                prevKunai = 0;
                break;
            case 1 :
                kunaiUI2.fillAmount = (combatStateManager.kunaiTimer / combatStateManager.kunaiChargeCooldown);
                if (prevKunai == 2)
                {
                    kunaiUI3.fillAmount = 0f;
                }
                prevKunai = 1;
                break;
            case 2 :
                kunaiUI3.fillAmount = (combatStateManager.kunaiTimer / combatStateManager.kunaiChargeCooldown);
                prevKunai = 2;
                break;
        }
    }
}
