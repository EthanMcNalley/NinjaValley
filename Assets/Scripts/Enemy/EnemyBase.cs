using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;
    Coroutine stateRoutine;
    Transform playerPos;
    public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint targetPoints;
    private NavMeshAgent agent;
    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    bool isWalking;
    public float distToPlayer;
    public GameObject player, target;
    public GameObject hitBox;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SwitchState(EnemyState.Patrol);
        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            targetPoints = thePath[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(thePath.Count);
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(currentState == EnemyState.Patrol)
        {
            rotateTowardsTarget();
            moveForward();
        }
    }

    void rotateTowardsTarget()
    {
        if (target == null) return;
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = targetPoints.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForward()
    {
        animator.SetBool("Moving", true);
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, targetPoints.pos);
        if (distanceToTarget < stepSize)
        {
            return;
        }

        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }


    IEnumerator ChaseState()
    {
        
        Debug.Log("Chase");
        animator.SetBool("Moving", true);
        agent.destination = target.transform.position;
        if (transform.position == target.transform.position)
            if (distToPlayer > 7f)
            {
                SwitchState(EnemyState.Patrol);
                yield break;


            }
        yield return null;
    }

    IEnumerator AttackState()
    {
        Debug.Log("Attack");

        while (currentState == EnemyState.Attack)
        {
            Attack();
            yield return null;
        }
    }

    IEnumerator PatrolState()
    {
        Debug.Log("Patrol");
        
        
        if (distToPlayer <= 7f)
        {
            SwitchState(EnemyState.Chase);
            yield break;


        }
        yield return null;
    }

    void SwitchState(EnemyState newState)
    {
        if (stateRoutine != null)
            StopCoroutine(stateRoutine);

        currentState = newState;

        // Start matching coroutine
        switch (currentState)
        {
            case EnemyState.Patrol:
                stateRoutine = StartCoroutine(PatrolState());
                break;

            case EnemyState.Chase:
                stateRoutine = StartCoroutine(ChaseState());
                break;

            case EnemyState.Attack:
                stateRoutine = StartCoroutine(AttackState());
                break;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void HitBoxOn()
    {
        hitBox.SetActive(true);
    }

    public void HitBoxOff()
    {
        hitBox.SetActive(false);
    }

    public void stateSwitch()
    {
        SwitchState(EnemyState.Chase);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.name.Contains("Point"))
        {
            Debug.Log("hit");
            targetPoints = pathManager.GetNextTarget();
        }
    }
}
