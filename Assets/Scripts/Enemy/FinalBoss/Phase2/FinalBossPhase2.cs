using UnityEngine;

public class FinalBossPhase2 : BossManager
{
    public GameObject hpCanvus;
    public BossState bossIdle = new BossPhase2Idle();
    BossState bossBeamAttack = new BossPhase2BeamAttack();
    BossState bossHowlingAttack = new BossPhase2HowlingAttack();
    BossState bossAirAttack = new BossPhase2AirAttack();
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
    public float rotationSpeed = 10f;
    public float distToPlayer { get; private set; }

    [Header("Attack Pattern")]
    public int normalAttacked;
    private bool attackAlternate;

    [Header("Music&Sounds")]
    public MusicEnum bossPhase2Music;
    public MusicEnum silent;

    public GameObject death_spawn;
    private bool dead;

    protected override void Start()
    {
        currentState = bossIdle;
        base.Start();
        beamVFX.SetActive(false);
        beamHitbox.SetActive(false);
        howlingHitbox.SetActive(false);
        airAttackHitBox.SetActive(false);
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
        // Always face player
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Pattern: beam → howling → air → repeat, break resets
    public void ChoseAttack()
    {
        if (currentState != bossIdle)
        {
            SwitchState(bossIdle);
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

    // --- Hitbox / VFX helpers (called by animation events) ---

    public void ActivateBeamVFX() => beamVFX.SetActive(true);
    public void DeactivateBeamVFX() => beamVFX.SetActive(false);

    public void EnableBeamHitBox() => beamHitbox.SetActive(true);
    public void DisableBeamHitBox() => beamHitbox.SetActive(false);

    public void EnableHowlingHitBox()
    {
        howlingHitbox.SetActive(true);
        howlingEffect.Play();
    }
    public void DisableHowlingHitBox() => howlingHitbox.SetActive(false);

    public void EnableAirAttackHitBox() => airAttackHitBox.SetActive(true);
    public void DisableAirAttackHitBox() => airAttackHitBox.SetActive(false);

    // --- Perfect dodge (called by animation events) ---

    public void SetPlayerPerfectDodgeTrue()
    {
        if (dead) return;
        // hook into your BossAttackScript dodge windows here
    }

    public void SetPlayerPerfectDodgeFalse()
    {
        if (dead) return;
        // hook into your BossAttackScript dodge windows here
    }

    public void PlaySound(string soundName)
    {
        if (dead) return;
        AudioManager.instance.PlayOneShot(soundName, transform.position);
    }

    // --- Shadow assassin slow ---

    void OnShadowStart() => GetComponent<Animator>().speed = 0.1f;
    void OnShadowEnd() => GetComponent<Animator>().speed = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();
        //AudioManager.instance.SetMusicArea(bossPhase2Music);
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