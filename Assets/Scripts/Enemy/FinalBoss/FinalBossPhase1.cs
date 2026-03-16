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
    [SerializeField]private PoopAttack lastPoopAttack;
    
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
    //misc stuff
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
