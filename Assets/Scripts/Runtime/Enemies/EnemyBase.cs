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
    [SerializeField] private Rigidbody rb;
    public Animator animator;

    [Space]
    [Header("Enemy stats")]
    [SerializeField] private EnemyProfileSO enemyProfile;

    protected GameObject player;

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
    private float watchingDuration;

    #region Wwise Event
    private AK.Wwise.Event attackWEvent;
    private AK.Wwise.Event chaseWEvent;
    private AK.Wwise.Event runWEvent;
    private AK.Wwise.Event watchWEvent;

    private AK.Wwise.Event hitWEvent;
    private AK.Wwise.Event deathWEvent;
    #endregion

    protected Action Attack;
    protected Action Chase;

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
    void Update()
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
                        // Attack
                        Debug.Log("Attack");
                        //Animations

                        animator.SetTrigger("attack");
                        // Post Wwise Event
                        attackWEvent?.Post(gameObject);
                        // Cooldown attack gestion
                        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
                        // Attack action
                        Attack?.Invoke();
                        // Start running if behaviour need
                        if (run)
                        {
                            agent.isStopped = true;
                            Invoke("EnableAgent", 1.5f);
                            running = true;
                            agent.speed = runSpeed;
                            if (runWEvent != null) runWEvent.Post(gameObject);
                            agent.SetDestination(GetRunningPoint());
                        }
                    }
                }
                else if (chase) // If the behaviour wants to chase the player
                {
                    if (!detected && distance < detectionRange) detected = true; // The enemy needs to detect the player
                    else if (detected && distance < chaseRange) // Then if the player is still in a range, it will chase him
                    {
                        // Raycast verification
                        RaycastHit hit;
                        Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, chaseRange, playerMask);
                        if (hit.transform != null)
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                // Chase
                                chasing = true;
                                Chase?.Invoke();
                            }
                        }
                    }
                }
            }
        }
    }

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
        watchingDuration = enemyProfile.watchingDuration;

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
        watchWEvent = enemyProfile.watchWEvent;
        hitWEvent = enemyProfile.hitWEvent;
        deathWEvent = enemyProfile.deathWEvent;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.DrawWireSphere(transform.position, rangeToAttack);
    }

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

    public void DamagePlayerTouched()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
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

        if (GetDistanceFromPlayer() >= 2.5f) timeToAttack = Time.timeSinceLevelLoad;
    }
    //public void ThrowProjectile()
    //{
    //    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //    Projectile proj = projectile.GetComponent<Projectile>();
    //    proj.direction = (player.transform.position - transform.position).normalized;
    //}

    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
    }
    private float GetDistanceFromPosition(Vector3 target)
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z));
    }

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

    private void EnableAgent() { agent.isStopped = false; }
    private void DisableAgent() { agent.isStopped = true; }
    private void StopWatchingPlayer() { agent.speed = movementSpeed; }
    private void WatchPlayer() { transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up); watchWEvent?.Post(gameObject); }
}
