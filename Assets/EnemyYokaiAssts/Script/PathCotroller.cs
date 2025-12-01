using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PathController : MonoBehaviour
{
    Transform playerPos;
    
    //public PathManager pathManager;
    public float waitTime = 2.5f;
    Waypoint targetPoints;
    private NavMeshAgent agent;
    public float movementSpeed;
    public float RotateSpeed;
    public Animator animator;
    bool isWalking;
    public float distToPlayer;
    public GameObject player, target;
    public GameObject hitBox;
    private bool canMove = true, isAttacking = false, isChasing = false;
    public GameObject[] patrolPoints;
    public int prevIndex = -1;
    public Transform rotateNode;
    
    public float shadowSlow = 0.1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (patrolPoints != null)
        {
            target = patrolPoints[0];
            
        }
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = movementSpeed;
    }

    private void OnEnable()
    {
        CombatEvents.ShadowAssassinStarted += OnShadowStart;
        CombatEvents.ShadowAssassinEnded += OnShadowEnd;
    }

    private void OnDisable()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
    }

    private void OnDestroy()
    {
        CombatEvents.ShadowAssassinStarted -= OnShadowStart;
        CombatEvents.ShadowAssassinEnded -= OnShadowEnd;
    }

    void OnShadowStart()
    {
        animator.SetFloat("Speed", shadowSlow);
        agent.speed = movementSpeed * shadowSlow;
        agent.angularSpeed *= shadowSlow;
    }

    void OnShadowEnd()
    {
        animator.SetFloat("Speed", 1f);
        agent.speed /= shadowSlow;
        agent.angularSpeed /= shadowSlow;
    }
    
    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (canMove || isAttacking == false)
        {
            agent.destination = target.transform.position;
            animator.SetBool("Moving", true);
        }
        if(canMove == false && isChasing == false)
        {
            animator.SetBool("Moving", false);
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                int a = Random.Range(0, patrolPoints.Length);

                // keep picking until it's not the same as prevIndex
                while (a == prevIndex && patrolPoints.Length > 1)
                {
                    a = Random.Range(0, patrolPoints.Length);
                }

                prevIndex = a;
                target = patrolPoints[a];
                canMove = true;
                waitTime = 2.5f;
            }
        }

        else if (canMove == true && isChasing == true)
        {
            target = player;
        }
        if (distToPlayer <= 10f)
        {
            isChasing = true;

        }

        else
        {
          

            isChasing = false;
        }
        //rotateTowardsTarget();
    }


    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        agent.speed = 0;
    }

    public void HitBoxOn()
    {
        hitBox.SetActive(true);
    }

    public void HitBoxOff()
    {
        hitBox.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (isChasing == false)
        {
            if (other.gameObject.tag == "Point")
            {
                canMove = false;

            }
        }

        if (other.gameObject.tag == "Player")
        {
            Attack();
            isAttacking = true;
        }
    }

    public void checkAttack()
    {
        isAttacking = false;
        agent.speed = movementSpeed;
        animator.ResetTrigger("Attack");
    }
}
