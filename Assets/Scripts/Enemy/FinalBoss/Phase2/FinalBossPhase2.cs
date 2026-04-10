using System;
using UnityEngine;
using FMODUnity;

public class FinalBossPhase2 : BossManager
{
    public GameObject hpCanvus;
    public BossState bossIdle = new BossPhase2Idle();
    BossState bossBeamAttack = new BossPhase2BeamAttack();
    BossState bossHowlingAttack = new BossPhase2HowlingAttack();
    BossState bossAirAttack = new BossPhase2AirAttack();
    BossState bossRealmAttack = new BossPhase2RealmAttack();
    BossState bossBreak = new BossPhase2Break();
    private BossState bossPhaseTransition = new BossPhase2Transition();

    [Header("References")]
    public GameObject beamVFX;
    public GameObject beamHitbox;
    public GameObject howlingHitbox;
    public GameObject airAttackHitBox;
    public ParticleSystem howlingEffect;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 1f;
    public float distToPlayer { get; private set; }
    public Vector3 playerCurrentPos;

    [Header("Attack Pattern")]
    public int normalAttacked;
    private bool attackAlternate;
    
    [Header("Realm Attack")]
    public GameObject realmPortal1;
    public GameObject Gate1, Gate2;
    public GameObject realmPortal2;
    public GameObject Gate3, Gate4;
    public Transform realmReturnPosition;
    [HideInInspector] public bool realmAttackActive;
    [HideInInspector] public GameObject activeRealmPortal;
    [HideInInspector] public GameObject activeGate1, activeGate2;
    private bool realm1Triggered;
    private bool realm2Triggered;
    public GameObject barrier;
    public GameObject brush1, brush2;
    public static event Action OnRealmEnemyKilled;
    public static void RealmEnemyKilled() => OnRealmEnemyKilled?.Invoke();

    [Header("Music&Sounds")]
    public MusicEnum bossPhase2Music;
    public MusicEnum silent;
    public EventReference howlSound;
    public EventReference beamSound;
    public EventReference biteSound;
    

    public GameObject death_spawn;
    private bool dead;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth =  player.GetComponent<HealthSystem>();
    }

    protected override void Start()
    {
        currentState = bossIdle;
        base.Start();
        beamVFX.SetActive(false);
        beamHitbox.SetActive(false);
        howlingHitbox.SetActive(false);
        airAttackHitBox.SetActive(false);
        realmPortal1.SetActive(false);
        realmPortal2.SetActive(false);
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    protected override void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (currentState != bossBreak && bossHpSystem.breakState == true)
        {
            SwitchState(bossBreak);
        }

        base.Update();
    }

    private void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void ChoseAttack()
    {
        if (currentState != bossIdle)
        {
            SwitchState(bossIdle);
            return;
        }
 
        float hp = bossHpSystem.checkHealthPercent();
 
        if (!realm1Triggered && hp <= 0.66f)
        {
            realm1Triggered = true;
            activeRealmPortal = realmPortal1;
            activeGate1 = Gate1;
            activeGate2 = Gate2;
            SwitchState(bossRealmAttack);
            return;
        }
 
        if (!realm2Triggered && hp <= 0.33f)
        {
            realm2Triggered = true;
            activeRealmPortal = realmPortal2;
            activeGate1 = Gate3;
            activeGate2 = Gate4;
            SwitchState(bossRealmAttack);
            return;
        }
 
        if (normalAttacked < 1)
        {
            normalAttacked++;
            SwitchState(bossBeamAttack);
        }
        else if (!attackAlternate)
        {
            attackAlternate = true;
            SwitchState(bossHowlingAttack);
        }
        else
        {
            normalAttacked = 0;
            attackAlternate = false;
            SwitchState(bossAirAttack);
        }
    }
    
    public void OnRealmComplete()
    {
        player.transform.position = realmReturnPosition.position;
        realmAttackActive = false;
        SwitchState(bossIdle);
    }

    public void ActivateBeamVFX() => beamVFX.SetActive(true);
    public void DeactivateBeamVFX() => beamVFX.SetActive(false);

    public void EnableBeamHitBox()
    {
        AudioManager.instance.PlayOneShot(beamSound, transform.position);
        beamHitbox.SetActive(true);
    }
    public void DisableBeamHitBox() => beamHitbox.SetActive(false);

    public void EnableHowlingHitBox()
    {
        howlingHitbox.SetActive(true);
        AudioManager.instance.PlayOneShot(howlSound, transform.position);
        howlingEffect.Play();
    }
    public void DisableHowlingHitBox() => howlingHitbox.SetActive(false);

    public void EnableAirAttackHitBox()
    {
        AudioManager.instance.PlayOneShot(biteSound, transform.position);
        airAttackHitBox.SetActive(true);
    }

    public void DisableAirAttackHitBox() => airAttackHitBox.SetActive(false);
    
    public void SetPlayerPerfectDodgeTrue()
    {
        if (dead) return;
        player.GetComponent<CombatStateManager>().SetPerfectDodgeWindow(true);
    }

    public void SetPlayerPerfectDodgeFalse()
    {
        if (dead) return;
        player.GetComponent<CombatStateManager>().SetPerfectDodgeWindow(false);
    }

    public void SetPlayerCurrentPos()
    {
        playerCurrentPos = player.transform.position;
    }

    public void PlaySound(string soundName)
    {
        if (dead) return;
        AudioManager.instance.PlayOneShot(soundName, transform.position);
    }

    void OnShadowStart() => GetComponent<Animator>().speed = 0.1f;
    void OnShadowEnd() => GetComponent<Animator>().speed = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();
        AudioManager.instance.SetMusicArea(bossPhase2Music);
        brush1.SetActive(true);
        brush2.SetActive(true);
        DeactivateBeamVFX();
        DisableBeamHitBox();
        DisableHowlingHitBox();
        DisableAirAttackHitBox();
        SetPlayerPerfectDodgeFalse();
        NewMovement.revive_position = realmReturnPosition.position;
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        dead = true;
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        if (death_spawn != null) death_spawn.SetActive(true);
    }
}