using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyUnit : MonoBehaviour
{
    private enum State
    {
        Idle,
        Looking,
        Chasing,
        Summoning,
        Attacking,
        Running,
        Watching, 
        Staggered,
        Dead
    }
    private State state;
    private State lastState;

    public EnemyProfileSO enemyProfile;

    public NavMeshAgent agent;
    public LayerMask navMeshMask;
    public Rigidbody rb;

    public Transform attackPoint;
    public LayerMask playerMask;
    private Transform player;

    private float stopDistance;
    private Action onActionFinished;

    private bool stagerred;
    private float timer;
    public bool attacking;
    public float attackTimer;
    private bool chasing;
    private bool running;
    private bool watching;
    public Action HandleSpawn;
    private Vector3 runStartPosition;

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
                HandleAttack();
                break;
            case State.Running:
                HandleRunning();
                break;
            case State.Watching:
                HandleWatching();
                break;
            case State.Staggered:
                HandleStagger();
                break;
        }
        if(attackTimer >= 0) { attackTimer -= Time.deltaTime; }
    }

    #region Unit methods
    // UNIT METHODS ---------------------------------------------------------------------
    public bool IsIdle() { return state == State.Idle; }
    public void ChaseThePlayer(float stopDistance, Action onArrivedAtPosition)
    {
        if (!chasing)
        {
            enemyProfile.chaseWEvent?.Post(gameObject);
            chasing = true;
        }
        agent.SetDestination(player.position);
        this.stopDistance = stopDistance;
        onActionFinished = onArrivedAtPosition;
        onActionFinished += () => chasing = false;
        state = State.Chasing;
    }
    public void LookForPlayer(Action onPlayerFound)
    {
        onActionFinished = onPlayerFound;
        state = State.Looking;
    }

    public void Summon(Action onSummonFinished)
    {
        onActionFinished = onSummonFinished;
        state = State.Summoning;
    }

    public void RunFromPlayer(float stopDistance,Action onStoppedRunning)
    {
        if (!running)
        {
            running = true;
            enemyProfile.runWEvent?.Post(gameObject);
            runStartPosition = player.position;
            agent.SetDestination(GetRunningPoint());
            onActionFinished = onStoppedRunning;
            onActionFinished += () => running = false;
            this.stopDistance = stopDistance;
            state = State.Running;
        }
    }

    public void WatchThePlayer(Action onStoppedWatching)
    {
        if (!watching) { enemyProfile.watchWEvent?.Post(gameObject); watching = true; }
        if(timer < 0)
        {
            timer = UnityEngine.Random.Range(enemyProfile.watchingDurationMinMax.x, enemyProfile.watchingDurationMinMax.y);
            onActionFinished = onStoppedWatching;
            onActionFinished += () => watching = false;
            state = State.Watching;
        }
    }

    public void GetStaggered(float duration, Action onStaggerFinished)
    {
        if (!stagerred)
        {
            lastState = state;
            attacking = false;
            running = false;
            stagerred = true;
            agent.SetDestination(transform.position);

            timer = enemyProfile.staggerDuration;
            onActionFinished = onStaggerFinished;
            onActionFinished += () => stagerred = false;
            state = State.Staggered;
        }
    }

    public void Attack(float stationaryDuration ,Action onAttackFinished) 
    {
        if (!attacking)
        {
            attacking = true;
            attackTimer = enemyProfile.attackCooldown;
            timer = stationaryDuration;
            onActionFinished = onAttackFinished;
            onActionFinished += () => attacking = false;
            state = State.Attacking; 
        }
    }
    public void Die()
    {
        state = State.Dead;
    }
    #endregion

    #region Handle methods
    // HANDLE METHODS --------------------------------------------------------------------- 
    private void HandleLooking()
    {
        if (GetDistanceFromPlayer() < enemyProfile.detectionRange)
        {
            if (isPlayerVisible(enemyProfile.detectionRange))
            {
                state = State.Idle; onActionFinished?.Invoke(); onActionFinished = null;
            }
        }
    }
    private void HandleChase()
    {
        if (GetDistanceFromPlayer() < stopDistance)
        {
            Debug.Log("try to aim");
            if (isPlayerAimable())
            {
                Debug.Log("aimable");
                state = State.Idle;
                agent.SetDestination(transform.position);

                if (onActionFinished != null)
                {
                    onActionFinished?.Invoke();
                    onActionFinished = null;
                }
            }
        }
    }
    private void HandleRunning()
    {
        if (GetDistanceFromPosition(runStartPosition) > enemyProfile.runningRange)
        {
            state = State.Idle;
            agent.SetDestination(transform.position);
            onActionFinished?.Invoke();
            onActionFinished = null;
        }
        else if (GetDistanceFromPosition(agent.destination) < stopDistance) { agent.SetDestination(GetRunningPoint()); }
    }
    private void HandleWaiting()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) { state = State.Idle; onActionFinished?.Invoke(); onActionFinished = null; }
    }
    private void HandleAttack()
    {
        HandleWaiting();
    }
    private void HandleWatching()
    {
        HandleWaiting();
        // addd stgh
    }
    private void HandleStagger()
    {
        HandleWaiting();
        if(timer < 0) { state = State.Chasing; }
    }
    #endregion

    #region Cast checks methods
    // CAST CHECKS ---------------------------------------------------------------------------
    public bool isPlayerVisible(float range)
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
    public bool isPlayerAimable()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 0.2f, dir, enemyProfile.rangeToAttack);
        if (hits.Length > 0)
        {
            Debug.LogWarning(hits.Length);
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.isTrigger)
                {
                    Debug.LogWarning(hit.transform.name + " :  " + hit.transform.tag +"----");
                    string tag = hit.transform.tag;
                    if (tag == "Player")
                    {
                        //Debug.Log("aimable"); 
                        return true;
                    }
                    else return false;
                }
            }
            Debug.LogWarning("hits finished");
        }
        return false;
    }
    #endregion

    #region GetDistance methods
    // GET DISTANCE METHOD --------------------------------------------------------------------
    public float GetDistanceFromPlayer()
    {
        if (player == null) return 10 ^ 5;
        return Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
    }
    public float GetDistanceFromPosition(Vector3 target)
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z));
    }
    #endregion
    private Vector3 GetRunningPoint()
    {
        Vector3 runningPointPos = Vector3.zero;

        //Debug.Log(direction);
        //Debug.Log(transform.position + new Vector3(direction.x, 0, direction.y) * runningRange);
        for(int i = 0; i <10;i++)
        {
            Vector3 direction = transform.position - player.transform.position;
            direction.y = 0;
            direction.Normalize();

            direction = RandomizeDirection(direction);

            runningPointPos = transform.position + direction * enemyProfile.runningRange;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(runningPointPos, out hit, enemyProfile.runningRange / 4, navMeshMask))
            {
                return hit.position;
            }
        }
        Debug.LogWarning("Running point not found");
        return transform.position;
    }
    private Vector3 RandomizeDirection(Vector3 direction)
    {
        return new Vector3(direction.x + UnityEngine.Random.Range(-direction.x, direction.x), 0,direction.z + UnityEngine.Random.Range(-direction.z,direction.z));
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyUnit))] public class EnemyUnitInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // Null
        GUILayout.Space(10);
        EditorGUILayout.HelpBox("Please refer the variables of the EnemyAI script", MessageType.Info);
    }
}
#endif
