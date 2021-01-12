using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("Nav Vars")]
    public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private LayerMask navMeshMask;

    [Space]
    [Header("Components Vars")]
    [SerializeField] protected Rigidbody rb;
    public Animator animator;

    [Space]
    [Header("Enemy stats")]
    [SerializeField] private EnemyProfileSO enemyProfile;

    protected GameObject player;

    #region Animations Actions
    protected Action attackAnimation;
    protected Action hitAnimation;
    #endregion

    #region EnemyProfile vars
    private int maxHealth;
    private int health;
    private bool dead;
    public bool pause;

    // Chase vars
    private bool chase;
    protected bool chasing;
    protected float detectionRange = 10f;
    private bool detected;
    protected float chaseRange = 15f;

    // Run vars
    private bool run;
    private bool running;
    private float runningRange;

    private bool range;
    protected GameObject projectilePrefab;

    private float rangeToAttack = 2f;
    // The point where colliders will be detected for the attack
    [SerializeField] private Transform attackPoint;
    private float attackRange = 0.5f;
    // Layer Mask used for the colliders detecttion
    [SerializeField] private LayerMask playerMask;

    protected float attackCooldown;
    protected float timeToAttack;
    private bool backstab;

    private float movementSpeed = 2f;
    private float runSpeed;
    private Vector2 watchingDurationMinMax;

    #region Wwise Event
    private AK.Wwise.Event attackWEvent;
    private string hitPlayerWEventSwitch;
    private AK.Wwise.Event chaseWEvent;
    private AK.Wwise.Event runWEvent;
    private float timeToPostRunEvent;
    private AK.Wwise.Event watchWEvent;

    private AK.Wwise.Event getHitWEvent;
    private AK.Wwise.Event deathWEvent;

    #endregion

    public Action Attack;
    protected Action Chase;
    #endregion

    #region Monobehaviours methods
    private void Awake()
    {
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        // Reference the player
        player = PlayerHelper.instance.gameObject;
        // Use the vars of the EnemyProfile scriptable object
        GetProfileFromSo();

        // Set agent speed
        agent.speed = movementSpeed;
        // Set cooldown attack
        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!dead && !pause)
        {
            // Moving animation handler
            if (agent.velocity != Vector3.zero) { animator.SetBool("moving", true); }
            else { animator.SetBool("moving", false); }

            float distance = GetDistanceFromPlayer();
            chasing = false;
            //Debug.Log(distance);

            if (run && running) // If the behaviour wants to run and the enemy is running
            {
                if (distance >= runningRange)
                {
                    // Stop running
                    running = false;
                    agent.speed = 0;
                    DisableAgent();
                    //agent.SetDestination(player.transform.position);

                    Invoke(nameof(WatchPlayer), 1f);
                    float watchingDuration = UnityEngine.Random.Range(watchingDurationMinMax.x, watchingDurationMinMax.y);
                    Invoke(nameof(StopWatchingPlayer), 1f + watchingDuration);
                    Invoke(nameof(EnableAgent), 1f + watchingDuration);
                    //transform.LookAt(player.transform);
                    //transform.eulerAngles
                }
                if (GetDistanceFromPosition(agent.destination) < 0.5f && distance < runningRange) // If agent destination reached and the enemy is too close from the player
                {
                    // Run to another point
                    agent.SetDestination(GetRunningPoint());
                }
            }
            else
            {
                if (distance < rangeToAttack && isPlayerVisible(rangeToAttack))
                {
                    if (timeToAttack <= Time.timeSinceLevelLoad) // Check attack cooldown condition
                    {
                        if (range) { if (!isPlayerAimable()) return; }
                        //Debug.Log("Enemy attack");
                        //Animations
                        attackAnimation?.Invoke();
                        // Post Wwise Event
                        attackWEvent?.Post(gameObject);
                        // Cooldown attack gestion
                        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
                        // Attack Action
                        if (range) { Attack?.Invoke(); } // if melee attack player detection and the func will be called during the attack animation
                        // Start running if behaviour need
                        if (run)
                        {
                            agent.isStopped = true;
                            Invoke(nameof(EnableAgent), 1.5f);
                            running = true;
                            agent.speed = runSpeed;
                            Invoke(nameof(PlayRunWEvent), timeToPostRunEvent);
                            agent.SetDestination(GetRunningPoint());
                        }
                    }
                    // Stop moving if range
                    if (range) { agent.isStopped = true; agent.SetDestination(transform.position); }
                }
                else if (chase) // If the behaviour wants to chase the player
                {
                    if (!detected && distance < detectionRange)
                    {
                        if (isPlayerVisible(detectionRange))
                        {
                            detected = true; // The enemy needs to detect the player
                            //Debug.Log("Player is detected");
                        }
                    }
                    else if (detected && distance < chaseRange) // Then if the player is still in a range, it will chase him
                    {
                        chasing = true;
                        agent.isStopped = false;
                        Chase?.Invoke();
                    }
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.DrawWireSphere(transform.position, rangeToAttack);
    }
    #endregion

    private void GetProfileFromSo()
    {
        // General vars
        maxHealth = enemyProfile.maxHealth;

        // Chase vars
        chase = enemyProfile.chase;
        detectionRange = enemyProfile.detectionRange;
        chaseRange = enemyProfile.chaseRange;

        // Run vars
        run = enemyProfile.run;
        runningRange = enemyProfile.runningRange;
        watchingDurationMinMax = enemyProfile.watchingDurationMinMax;

        // Range vars
        range = enemyProfile.range;
        projectilePrefab = enemyProfile.projectilePrefab;

        // Attack vars
        rangeToAttack = enemyProfile.rangeToAttack;
        attackRange = enemyProfile.attackRange;
        attackCooldown = enemyProfile.attackCooldown;
        backstab = enemyProfile.backstab;

        // Speed vars
        movementSpeed = enemyProfile.movementSpeed;
        runSpeed = enemyProfile.runSpeed;

        // Wwise events
        attackWEvent = enemyProfile.attackWEvent;
        chaseWEvent = enemyProfile.chaseWEvent;
        runWEvent = enemyProfile.runWEvent;
        timeToPostRunEvent = enemyProfile.timeToPostRunEvent;
        //PlayRunWEvent = () => runWEvent?.Post(gameObject);

        watchWEvent = enemyProfile.watchWEvent;
        getHitWEvent = enemyProfile.getHitWEvent;
        deathWEvent = enemyProfile.deathWEvent;
        hitPlayerWEventSwitch = enemyProfile.hitPlayerWEventSwitch;

    }

    #region Cast checks
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
        hits = Physics.SphereCastAll(transform.position, 0.2f, dir, rangeToAttack);
        if(hits.Length > 0) 
        { 
            foreach(RaycastHit hit in hits)
            {
                string tag = hit.transform.tag;
                if(tag == "Player") { Debug.Log("aimable"); return true; }
                else if(tag != "Ground" && tag != "Enemy") { return false; }
            }
        }
        return false;
    }
    #endregion

    #region Health methods
    public void TakeDamage()
    {
        Debug.Log("Damage took");
        health--;
        if (health <= 0) { Die(); }
        else
        {
            hitAnimation?.Invoke();
            getHitWEvent?.Post(gameObject);
        }
    }
    public void Die()
    {
        dead = true;
        animator.SetTrigger("dead");
        deathWEvent?.Post(gameObject);
        //Destroy(gameObject);
    }
    #endregion

    #region Attack methods
    public void DamagePlayerTouched()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
        if(hits.Length > 0)
        {
            if (hits[0].transform != null)
            {
                Debug.Log(hits[0].transform.name);
                Player_Movement player = hits[0].transform.GetComponent<Player_Movement>();
                if(player!= null) player.OnHit(hitPlayerWEventSwitch);
                if (backstab)
                {
                    float angle = Vector3.Angle(hits[0].transform.forward, transform.forward);

                    if (angle < 90f) {  } // faire truc de vies
                }
            }
        }
        else
        {
            Debug.Log("No gameobject touched with Player layer");
        }

        if (GetDistanceFromPlayer() >= 2.5f) timeToAttack = Time.timeSinceLevelLoad;
    }
    //public void ThrowProjectile()
    //{
    //    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //    Projectile proj = projectile.GetComponent<Projectile>();
    //    proj.direction = (player.transform.position - transform.position).normalized;
    //}
    #endregion

    #region GetDistance methods
    private float GetDistanceFromPlayer()
    {
        if (player == null) return 10^5;
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
        if (NavMesh.SamplePosition(runningPointPos, out hit, runningRange / 4, navMeshMask))
        {
            runningPointPos = transform.position + new Vector3(direction.x, 0, direction.y) * runningRange;
            //Debug.Log("Running point found");
        }
        else
        {
            //Debug.Log("Running point not found bruh");
        }
        return runningPointPos;

    }
    public void PlayRunWEvent() { runWEvent?.Post(gameObject); }

    #region Agent methods
    protected void EnableAgent() { agent.isStopped = false; }
    protected void DisableAgent() { agent.isStopped = true; }
    #endregion

    #region Watch methods
    private void StopWatchingPlayer() { agent.speed = movementSpeed; }
    private void WatchPlayer() { transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up); watchWEvent?.Post(gameObject); }
    #endregion
}
