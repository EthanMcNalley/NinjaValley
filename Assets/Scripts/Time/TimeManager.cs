using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeState time_state;
    private TimeState current_state = TimeState.NORMAL;
    private TimeState prev_state = TimeState.NORMAL;
    public float time_timer = 0.0f;
    public float time_slowed_down = 3.0f;
    public static float slowed_amount = 0.1f;
    public float slow_amount = 0.1f;
    public float refresh_time = 3.0f;
    public float refresh_timer = 0.0f;
    public GameObject volume;
    public Material time_slow_material;
    private Material instance_material;
    public FullScreenPassRendererFeature time_slow_renderer;
    private float time_size = 0.0f;
    public float material_rate = 3.0f;
    private GameObject[] anim_objects;
    
    private InputAction timeSlowAction;

    public GameObject terrainScannerPrefab;
    public GameObject player;
    public bool shadowActive, playerInCombat;
    public float scanDurration = 10f;
    public float scanSize = 500;
    
    private MusicState current_musicState = MusicState.NORMAL;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time_timer = time_slowed_down;
        time_state = TimeState.NORMAL;
        refresh_timer = refresh_time;

        if (InputSystem.actions)
        {
            timeSlowAction = InputSystem.actions.FindAction("TimeSlow");
        }
        
        anim_objects = GameObject.FindGameObjectsWithTag("Test");
        instance_material = new Material(time_slow_material);
        time_slow_renderer.passMaterial = instance_material;
        
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    
    private void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
        CombatEvents.PlayerInCombat += OnPlayerCombat;
        CombatEvents.PlayerInCombatEnded += OnPlayerCombatEnded;
    }

    private void OnDisable()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        CombatEvents.PlayerInCombat -= OnPlayerCombat;
        CombatEvents.PlayerInCombatEnded -= OnPlayerCombatEnded;
    }
    
    private void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
        CombatEvents.PlayerInCombat -= OnPlayerCombat;
        CombatEvents.PlayerInCombatEnded -= OnPlayerCombatEnded;
    }

    void OnPlayerCombat() => playerInCombat = true;
    void OnPlayerCombatEnded() => playerInCombat = false;
    void OnShadowStart() => shadowActive = true;
    void OnShadowEnd() => shadowActive = false;

    // Update is called once per frame
    void Update()
    {
        // if(volume.profile)){
        //     original_saturation_value = adjustments.saturation.value;
        // }
        if (time_state == TimeState.NORMAL)
        {
            if (time_size > 0)
            {
                time_size -= Time.deltaTime * material_rate;
            }

            if (refresh_timer >= refresh_time && timeSlowAction.triggered && NewMovement.time_able)
            {
                time_timer = 0f;
                refresh_timer = 0f;
                time_state = TimeState.SLOWED;
                InstantiateTerrainScanner();
            }
        }
        else if (time_state == TimeState.SLOWED)
        {
            if (time_size < 3f)
            {
                time_size += Time.deltaTime * material_rate;
            }
            //when duration ends
            if (time_timer >= time_slowed_down)
            {
                time_state = TimeState.NORMAL;
            }
            //only allow manual cancel when not in combat and shadow assassin
            if (!playerInCombat && !shadowActive && timeSlowAction.triggered)
            {
                refresh_timer = time_slowed_down - time_timer;
                time_timer = time_slowed_down;
                time_state = TimeState.NORMAL;
            }
        }
        
            /*if (time_state == TimeState.NORMAL){
                //volume.SetActive(false);
                if (time_size > 0)
                {
                    time_size -= Time.deltaTime * material_rate;
                } 

                if (refresh_timer >= refresh_time){
                    if (timeSlowAction.triggered){
                        time_timer = 0.0f;
                        refresh_timer = 0.0f;
                        time_state = TimeState.SLOWED;
                        InstantiateTerrainScanner();
                        //Time.fixedDeltaTime = 0.02f * slowed_amount;
                    }
                }
            }
            else if ((time_state == TimeState.SLOWED && playerInCombat && shadowActive)){
                if (time_size < 3){
                    time_size += Time.deltaTime * material_rate;
                }
                //volume.SetActive(true);
                // Time.timeScale = slowed_amount;
                //Debug.Log(time_timer);

                if (time_timer >= time_slowed_down){
                    time_state = TimeState.NORMAL;
                    //Time.fixedDeltaTime = 0.02f;
                    //Time.timeScale = 1.0f;
                    
                }
            }
            else if ((time_state == TimeState.SLOWED && !playerInCombat) || (time_state == TimeState.SLOWED && playerInCombat))
            {
                if (time_size < 3){
                    time_size += Time.deltaTime * material_rate;
                }
                //volume.SetActive(true);
                // Time.timeScale = slowed_amount;
                //Debug.Log(time_timer);

                if (time_timer >= time_slowed_down){
                    time_state = TimeState.NORMAL;
                    //Time.fixedDeltaTime = 0.02f;
                    //Time.timeScale = 1.0f;
                }
                
                if (timeSlowAction.triggered){
                    refresh_timer = time_slowed_down - time_timer;
                    time_timer = time_slowed_down;
                    time_state = TimeState.NORMAL;
                }
            }*/

        instance_material.SetFloat("_WipeSize", time_size);


        if (time_timer < time_slowed_down){
            time_timer = time_timer + Time.deltaTime;
        }

        else{
            if (refresh_timer < refresh_time){
                refresh_timer = refresh_timer + Time.deltaTime;
            }
        }

        if (time_state == TimeState.NORMAL){
            slowed_amount = 1.0f;
            if (current_musicState == MusicState.SLOWED)
            {
                AudioManager.instance.SetSlowTime(0f);
                //AudioManager.instance.musicEventInstance.setParameterByName("Ticking", 1);
                current_musicState = MusicState.NORMAL;
            }
        }

        else{
            slowed_amount = slow_amount;
            if (current_musicState == MusicState.NORMAL)
            {
                AudioManager.instance.SetSlowTime(1f);
                //AudioManager.instance.musicEventInstance.setParameterByName("Ticking", 0);
                current_musicState = MusicState.SLOWED;
            }
        }

        for (int i = 0; i < anim_objects.Length; i++){
            anim_objects[i].GetComponent<Animator>().SetFloat("Speed",  slowed_amount);
        }
    }

    void InstantiateTerrainScanner()
    {
        GameObject terrainScanner = Instantiate(terrainScannerPrefab, player.transform.position, Quaternion.identity);
        ParticleSystem ps = terrainScanner.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var main = ps.main;
            main.startLifetime = scanDurration;
            main.startSize = scanSize;
        }
        else
        {
            return;
        }
        
        Destroy(terrainScanner, scanDurration + 1);
    }

    public enum TimeState{
        NORMAL,
        SLOWED
    }

    public enum MusicState
    {
        NORMAL,
        SLOWED
    }

    public TimeState GetTimeState()
    {
        return time_state;
    }

    public void SlowTime(){

    }

    public void NormalTime(){

    }
}
