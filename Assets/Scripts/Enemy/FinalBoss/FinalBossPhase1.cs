using Unity.VisualScripting;
using UnityEngine;

public class FinalBossPhase1 : BossManager
{
    public BossState bossIdle = new BossIdle();
    public BossState bossPhase1NormalAttack = new BossPhase1NormalAttack();
    public BossState bossPhase1TileAttack = new BossPhase1TileAttack();
    public BossState bossPhase1DoorWordAttack = new BossPhase1DoorWordAttack();
    public BossState bossBreak = new BossBreak();

    protected override void Start()
    {
        currentState = bossIdle;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
