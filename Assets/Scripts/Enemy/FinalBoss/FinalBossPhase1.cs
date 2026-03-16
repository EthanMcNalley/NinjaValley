using System.Collections.Generic;
using UnityEngine;

public class FinalBossPhase1 : BossManager
{
    public Animator tileAttackAnimator;
    public BossState bossIdle = new BossIdle();
    BossState bossPhase1NormalAttack = new BossPhase1NormalAttack();
    BossState bossPhase1TileAttack = new BossPhase1TileAttack();
    BossState bossPhase1DoorWordAttack = new BossPhase1DoorWordAttack();
    BossState bossBreak = new BossBreak();
    
    //Normal Attack
    
    //poopAttack stuff (floor rising ink blobs)
    public PoopAttack[] poopAttacks1;
    public PoopAttack[] poopAttacks2;
    public PoopAttack[] poopAttacks3;
    private bool firstPoopAttack = true;
    private PoopAttack lastPoopAttack;
    
    //portal stuff
    public GameObject[] portals;
    [SerializeField]private GameObject lastPortal;
    public PortalIndicatorUI portalIndicatorUI;
    public List<Color> avaliablePortalColors;
    public List<Color> neededPortalColors;
    
    //normal attacks
    private int normalAttacked;
    
    protected override void Start()
    {
        currentState = bossIdle;
        base.Start();
    }
    
    protected override void Update()
    {
        if (currentState != bossBreak && bossHpSystem.breakState == true)
        {
            SwitchState(bossBreak);
        }

        base.Update();
    }
    
        
    public void ChoseAttack()
    {
        SwitchState(bossPhase1TileAttack);
    }
    
    
    public PoopAttack poopAttackPatternSelector()
    {
        if (firstPoopAttack)
        {
            firstPoopAttack = false;
            lastPoopAttack = poopAttacks1[0];
            return poopAttacks1[0];
        }
        
        float hp = bossHpSystem.checkHealthPercent();

        PoopAttack[] attackPool;
        if (hp > 0.7f)
        {
            attackPool = poopAttacks1;
        }
        else if (hp > 0.35f)
        {
            attackPool = poopAttacks2;
        }
        else
        {
            attackPool = poopAttacks3;
        }
        
        PoopAttack selected;
        do 
        {
            selected = attackPool[Random.Range(0, attackPool.Length)];
        } while (selected == lastPoopAttack && attackPool.Length > 1);

        lastPoopAttack = selected;
        return selected;
    }

    public List<Color> NeededPortalColors()
    {
        neededPortalColors.Clear();
        
        float hp = bossHpSystem.checkHealthPercent();
        int colorNeeded = 0;
        
        if (hp > 0.7f)
        {
            colorNeeded = 1;
        }
        else if (hp > 0.35f)
        {
            colorNeeded = 2;
        }
        else
        {
            colorNeeded = 3;
        }
        
        List<Color> shuffled = new List<Color>(avaliablePortalColors);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        
        for (int i = 0; i < colorNeeded; i++)
        {
            neededPortalColors.Add(shuffled[i]);
        }

        portalIndicatorUI.SetRequiredPortals(neededPortalColors);

        return neededPortalColors;
    }

    void PortalUsed(PortalFinal portal)
    {
        lastPortal = portal.gameObject;
        portalIndicatorUI.MarkPortalComplete(portal.portalColor);
    }
    
    //misc stuff
    protected override void OnEnable()
    {
        base.OnEnable();
                
        PortalFinal.OnPlayerTeleported += PortalUsed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        PortalFinal.OnPlayerTeleported -= PortalUsed;
    }
}



[System.Serializable]
public class PoopAttack
{
    public GameObject pointing;
    public int thePoop;

    public PoopAttack(GameObject pointing, int thePoop)
    {
        this.pointing = pointing;
        this.thePoop = thePoop;
    }
}
