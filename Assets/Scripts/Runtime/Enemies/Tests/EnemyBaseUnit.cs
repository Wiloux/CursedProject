using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseUnit : MonoBehaviour, IUnit
{
    private enum State
    {
        Idle,
        Looking,
        Chasing,
        Attacking,
        Running, //
        Watching, //
        Dead
    }
    private State state;

    [SerializeField] private EnemyProfileSO enemyProfile;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask navMeshMask;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerMask;
    private Transform player;

    private float stopDistance;
    private Action onActionFinished;

    private float timer;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHelper.instance.transform;
        agent.speed = enemyProfile.movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Chasing:
                HandleChase();
                break;
            case State.Looking:
                HandleLooking();
                break;
            case State.Attacking:
                break;
            case State.Running:
                HandleRunning();
                break;
            case State.Watching:
                HandleWatching();
                break;
        }
    }

    #region Unit methods
    // UNIT METHODS ---------------------------------------------------------------------
    public bool IsIdle() { return state == State.Idle; }
    public void ChaseThePlayer(Vector3 position, float stopDistance, Action onArrivedAtPosition)
    {
        agent.SetDestination(position);
        this.stopDistance = stopDistance;
        this.onActionFinished = onArrivedAtPosition;
        state = State.Chasing;
    }
    public void LookForPlayer(Action onPlayerFound)
    {
        onActionFinished = onPlayerFound;
        state = State.Looking;
    }

    public void RunFromPlayer(float stopDistance,Action onStoppedRunning)
    {
        agent.SetDestination(GetRunningPoint());
        onActionFinished = onStoppedRunning;
        this.stopDistance = stopDistance;
        state = State.Running;
    }

    public void WatchThePlayer(Action onStoppedWatching)
    {
        timer = UnityEngine.Random.Range(enemyProfile.watchingDurationMinMax.x, enemyProfile.watchingDurationMinMax.y);
        onActionFinished = onStoppedWatching;
        state = State.Watching;
    }

    public void Attack(Action onAttackFinished) { onActionFinished = onAttackFinished; state = State.Attacking; }
    #endregion

    #region Handle methods
    // HANDLE METHODS --------------------------------------------------------------------- 
    private void HandleLooking()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance < enemyProfile.detectionRange)
        {
            if (isPlayerVisible(enemyProfile.detectionRange))
            {
                state = State.Idle; onActionFinished?.Invoke(); onActionFinished = null;
            }
        }
    }
    private void HandleChase()
    {
        float playerDistance = Vector3.Distance(agent.destination, player.position);
        if (playerDistance < stopDistance)
        {
            if (isPlayerAimable())
            {
                if (onActionFinished != null)
                {
                    state = State.Idle;
                    onActionFinished?.Invoke();
                    onActionFinished = null;
                }
            }
        }
    }
    private void HandleRunning()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance > enemyProfile.runningRange)
        {
            state = State.Idle;
            onActionFinished?.Invoke();
            onActionFinished = null;
        }
        else
        {
            float destinationDistance = Vector3.Distance(transform.position, agent.destination);
            if (destinationDistance < stopDistance) { agent.SetDestination(GetRunningPoint()); }
        }
    }
    private void HandleWatching()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) { state = State.Idle; onActionFinished?.Invoke(); onActionFinished = null; }
    }
    #endregion

    #region Cast checks methods
    // CAST CHECKS ---------------------------------------------------------------------------
    private bool isPlayerVisible(float range)
    {
        bool boolean = false;
        RaycastHit hit;
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Physics.Raycast(transform.position, dir, out hit, range);
        Debug.DrawRay(transform.position, dir * range, Color.red, 0.1f);
        if (hit.transform != null)
        {
            //Debug.Log("Object hit by is : " + hit.transform.name);
            if (hit.transform.CompareTag("Player"))
            {
                //Debug.Log("The player is visible");
                boolean = true;
            }
        }
        return boolean;
    }
    private bool isPlayerAimable()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 0.2f, dir, enemyProfile.rangeToAttack);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                string tag = hit.transform.tag;
                if (tag == "Player")
                {
                    //Debug.Log("aimable"); 
                    return true;
                }
                else if (tag != "Ground" && tag != "Enemy") { return false; }
            }
        }
        return false;
    }
    #endregion

    #region GetDistance methods
    // GET DISTANCE METHOD --------------------------------------------------------------------
    private float GetDistanceFromPlayer()
    {
        if (player == null) return 10 ^ 5;
        return Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
    }
    private float GetDistanceFromPosition(Vector3 target)
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z));
    }
    #endregion
    private Vector3 GetRunningPoint()
    {
        Vector3 runningPointPos = transform.position;
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1.01f), UnityEngine.Random.Range(-1f, 1.01f)).normalized;

        //Debug.Log(direction);
        //Debug.Log(transform.position + new Vector3(direction.x, 0, direction.y) * runningRange);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(runningPointPos, out hit, enemyProfile.runningRange / 4, navMeshMask))
        {
            runningPointPos = transform.position + new Vector3(direction.x, 0, direction.y) * enemyProfile.runningRange;
            //Debug.Log("Running point found");
        }
        else
        {
            //Debug.Log("Running point not found bruh");
        }
        return runningPointPos;
    }

    public void DamagePlayerTouched()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, enemyProfile.attackRange, playerMask);
        if (hits.Length > 0)
        {
            if (hits[0].transform != null)
            {
                Debug.Log(hits[0].transform.name);
                Player player = hits[0].transform.GetComponent<Player>();
                if (player != null) player.OnHit(enemyProfile.hitPlayerWEventSwitch);
                if (enemyProfile.backstab)
                {
                    float angle = Vector3.Angle(hits[0].transform.forward, transform.forward);

                    if (angle < 90f) { } // faire truc de vies
                }
            }
        }
        else
        {
            Debug.Log("No gameobject touched with Player layer");
        }

        if (GetDistanceFromPlayer() >= 2.5f) attackTimer = enemyProfile.attackCooldown;
    }
}
