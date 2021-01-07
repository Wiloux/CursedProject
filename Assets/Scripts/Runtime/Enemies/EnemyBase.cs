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

    private float attackCooldown;
    private float timeToAttack;
    private bool backstab;

    private float movementSpeed = 2f;
    private float runSpeed;
    private Vector2 watchingDurationMinMax;

    #region Wwise Event
    private AK.Wwise.Event attackWEvent;
    private AK.Wwise.Event chaseWEvent;
    private AK.Wwise.Event runWEvent;
    private float timeToPostRunEvent;
    private AK.Wwise.Event watchWEvent;

    private AK.Wwise.Event hitWEvent;
    private AK.Wwise.Event deathWEvent;
    #endregion

    protected Action Attack;
    protected Action Chase;
    #endregion

    #region Monobehaviours methods
    // Start is called before the first frame update
    public virtual void Start()
    {
        // Use the vars of the EnemyProfile scriptable object
        GetProfileFromSo();

        // Set agent speed
        agent.speed = movementSpeed;
        // Set cooldown attack
        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
        // Reference the player
        player = PlayerHelper.instance.gameObject;
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

                    Invoke("WatchPlayer", 1f);
                    float watchingDuration = UnityEngine.Random.Range(watchingDurationMinMax.x, watchingDurationMinMax.y);
                    Invoke("StopWatchingPlayer", 1f + watchingDuration);
                    Invoke("EnableAgent", 1f + watchingDuration);
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
                if (distance < rangeToAttack)
                {
                    if (timeToAttack <= Time.timeSinceLevelLoad) // Check attack cooldown condition
                    {
                        if (isPlayerVisible(rangeToAttack))
                        {
                            if (range) { if (!isPlayerAimable()) return; }
                            Debug.Log("Attack");
                            //Animations
                            animator.SetInteger("attackType", UnityEngine.Random.Range(0, 2));
                            animator.SetTrigger("attack");
                            // Post Wwise Event
                            attackWEvent?.Post(gameObject);
                            // Cooldown attack gestion
                            timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
                            // Stop moving if range
                            if (range) agent.isStopped = true;
                            // Attack Action
                            Attack?.Invoke();
                            // Start running if behaviour need
                            if (run)
                            {
                                agent.isStopped = true;
                                Invoke("EnableAgent", 1.5f);
                                running = true;
                                agent.speed = runSpeed;
                                Invoke("PlayRunWEvent", timeToPostRunEvent);
                                agent.SetDestination(GetRunningPoint());
                            }
                        }
                    }
                }
                else if (chase) // If the behaviour wants to chase the player
                {
                    if (!detected && distance < detectionRange)
                    {
                        if (isPlayerVisible(detectionRange))
                        {
                            detected = true; // The enemy needs to detect the player
                            Debug.Log("detected");
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
        hitWEvent = enemyProfile.hitWEvent;
        deathWEvent = enemyProfile.deathWEvent;

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
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("yes");
                boolean = true;
            }
        }
        return boolean;
    }
    private bool isPlayerAimable()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 1.25f, dir, rangeToAttack);
        if(hits.Length > 0) 
        { 
            foreach(RaycastHit hit in hits)
            {
                string tag = hit.transform.tag;
                if(tag == "Player") { return true; }
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
            animator.SetTrigger("hit");
            animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 3));
            hitWEvent?.Post(gameObject);
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
                if (backstab)
                {
                    float angle = Vector3.Angle(hits[0].transform.forward, transform.forward);
                    Debug.Log(angle);

                    if (angle < 90f) { Debug.Log("backstab = double damage"); animator.SetInteger("attackType", 2); }
                }
                else
                {
                    Debug.Log("player hit");
                    animator.SetInteger("attackType", UnityEngine.Random.Range(0, 2));
                }
            }
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

        Debug.Log(direction);
        Debug.Log(transform.position + new Vector3(direction.x, 0, direction.y) * runningRange);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(runningPointPos, out hit, runningRange / 4, navMeshMask))
        {
            runningPointPos = transform.position + new Vector3(direction.x, 0, direction.y) * runningRange;
            Debug.Log("found");
        }
        else
        {
            Debug.Log("bruh");
        }
        return runningPointPos;

    }
    public void PlayRunWEvent() { runWEvent?.Post(gameObject); }

    #region Agent methods
    private void EnableAgent() { agent.isStopped = false; }
    private void DisableAgent() { agent.isStopped = true; }
    #endregion

    #region Watch methods
    private void StopWatchingPlayer() { agent.speed = movementSpeed; }
    private void WatchPlayer() { transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up); watchWEvent?.Post(gameObject); }
    #endregion
}
