using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using FMODUnity;

public class FinalBossPhase1 : BossManager
{
    public Animator tileAttackAnimator;
    public GameObject hpCanvus;
    public BossState bossIdle = new BossPhase1Idle();
    BossState bossPhase1NormalAttack = new BossPhase1NormalAttack();
    BossState bossPhase1TileAttack = new BossPhase1TileAttack();
    BossState bossPhase1DoorWordAttack = new BossPhase1DoorWordAttack();
    BossState bossBreak = new BossPhase1Break();
    private BossState bossPhaseTransition = new BossPhase1Transition();
    
    [Header("Poop Attacks")]
    //poopAttack stuff (floor rising ink blobs)
    public PoopAttack[] poopAttacks1;
    public PoopAttack[] poopAttacks2;
    public PoopAttack[] poopAttacks3;
    private bool firstPoopAttack = true;
    private PoopAttack lastPoopAttack;
    public EventReference poopSound;

    [Header("Portal")]
    //portal stuff
    public GameObject[] portals;
    [SerializeField]private GameObject lastPortal;
    public GameObject portalTopIndicator;
    public PortalIndicatorUI portalIndicatorUI;
    public List<Color> avaliablePortalColors;
    public List<Color> neededPortalColors;
    public Animator[] slidingDoorAnimator;
    public GameObject barrier;
    
    [Header("Normal Attack")]
    //normal attacks
    public int normalAttacked;
    [SerializeField] private GameObject normalAttackPrefab;
    [SerializeField] private Vector3 normalAttackOffset;

    [Header("Music&Sounds")] 
    public MusicEnum bossPhase1Music;
    public MusicEnum silent;
    
    [Header("PhaseTransition")]
    public bool phaseTransition;
    public bool teleportPlayer;
    public Volume transitionVolume;
    [SerializeField] private float transitionTime;
    [SerializeField] private Transform phaseTransitionPosition;
    public EventReference transitionSound;

    public bool attackAlternate;
    
    protected override void Start()
    {
        currentState = bossIdle;
        base.Start();
        //StartCoroutine(VolumeChange());
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
        if (currentState != bossIdle)
        {
            SwitchState(bossIdle);
        }
        else if (normalAttacked < 1)
        {
            SwitchState(bossPhase1NormalAttack);
        }
        else if (!attackAlternate)
        {
            attackAlternate = true;
            SwitchState(bossPhase1TileAttack);
        }
        else if (currentState == bossPhase1TileAttack || currentState == bossIdle || currentState == bossBreak)
        {
            normalAttacked = 0;
            attackAlternate = false;
            SwitchState(bossPhase1DoorWordAttack);
        }
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

        portalIndicatorUI.SetRequiredPortals(neededPortalColors, avaliablePortalColors);

        return neededPortalColors;
    }

    void PortalUsed(PortalFinal portal)
    {
        if (currentState != bossPhase1DoorWordAttack) return;
        lastPortal = portal.gameObject;
        portalIndicatorUI.MarkPortalComplete(portal.portalColor);
    }

    public void InstantiateNormalAttack()
    {
        Instantiate(normalAttackPrefab, player.transform.position + normalAttackOffset, Quaternion.identity);
    }

    public void PhaseTransition()
    {
        if (phaseTransition) return;
        Debug.Log("Phase Transition");
        phaseTransition = true;
        hpCanvus.SetActive(false);
        AudioManager.instance.SetMusicArea(silent);
        StartCoroutine(VolumeChange());

    }

    IEnumerator VolumeChange()
    {
        var volume = Instantiate(transitionVolume, transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(volume.gameObject, SceneManager.GetSceneByName("RealPersistables"));

        AudioManager.instance.PlayOneShot(transitionSound, transform.position);
        while (volume.weight < 1)
        {
            volume.weight = Mathf.MoveTowards(volume.weight, 1, Time.deltaTime * transitionTime);
            yield return null;
        }
        
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("FinalBossPhase2", LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(3f);
        asyncOp.allowSceneActivation = false;
        while (asyncOp.progress < 0.9f)
        {
            yield return null;
        }
        asyncOp.allowSceneActivation = true;
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("FinalBossPhase2"));
        
        teleportPlayer = true;
        
        yield return null;
        SceneManager.UnloadSceneAsync("FinalArea");
    }

    private void LateUpdate()
    {
        if (phaseTransition && teleportPlayer)
        {
            Debug.Log("Player Teleported");
            teleportPlayer = false;
            player.transform.position = phaseTransitionPosition.position;
        }
    }


    //misc stuff
    protected override void OnEnable()
    {
        base.OnEnable();
        normalAttacked = 0;
        firstPoopAttack = true;
        attackAlternate = false;
        SwitchState(bossIdle);
        AudioManager.instance.SetMusicArea(bossPhase1Music);
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
