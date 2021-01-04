using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("Nav Vars")]
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyStatsSO enemyStats;
    //[SerializeField] private Animator animator;
    //[SerializeField] protected Player player;
    protected GameObject player;

    private int maxHealth;
    private int health;
    private bool dead;

    // Chase vars
    private bool chase;
    protected bool chasing;
    protected float detectionRange = 10f;
    private bool detected;
    protected float chaseRange = 15f;

    // Run vars
    private bool run;
    private bool running;
    [SerializeField] private Transform runningPointsParent;
    private Transform latestRunningPoint;
    private float runningRange;

    private bool range;
    protected GameObject projectilePrefab;

    private float rangeToAttack = 2f;
    [Tooltip("The point where colliders will be detected for the attack")]
    [SerializeField] private Transform attackPoint;
    private float attackRange = 0.5f;
    [Tooltip("Layer Mask used for the colliders detecttion")]
    [SerializeField] private LayerMask playerMask;

    private float attackCooldown;
    private float timeToAttack;

    private float movementSpeed = 2f;
    private float runSpeed;

    protected Action Attack;
    protected Action Chase;

    // Start is called before the first frame update
    public virtual void Start()
    {
        GetStatsFromSo();

        agent.speed = movementSpeed;
        //player = GameObject.FindObjectOfType<Player>();
        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
        player = PlayerHelper.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) player = PlayerHelper.instance.gameObject;
        if (!dead)
        {
            if(agent.velocity != Vector3.zero)
            {
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }
            Debug.Log(agent.isStopped);
            float distance = GetDistanceFromPlayer();
            chasing = false;
            //Debug.Log(distance);
            if(run && running)
            {
                if(distance >= runningRange)
                {
                    running = false;
                    agent.speed = movementSpeed;
                }
                if(GetDistanceFromPosition(agent.destination) < 0.5f && distance < runningRange)
                {
                    agent.SetDestination(GetRunningPoint());
                }
            }
            else
            {
                if(distance < rangeToAttack)
                {
                    if(timeToAttack <= Time.timeSinceLevelLoad)
                    {
                        // Attack
                        animator.SetInteger("attackType", UnityEngine.Random.Range(0, 3));
                        animator.SetTrigger("attack");
                        Debug.Log("Attack");
                        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
                        Attack?.Invoke();
                        if (run)
                        {
                            running = true;
                            agent.speed = runSpeed;
                            agent.SetDestination(GetRunningPoint());
                        }
                    }
                }
                else if(chase)
                {
                    if (!detected && distance < detectionRange) detected = true;
                    else if(detected && distance < chaseRange)
                    {
                        // Chase
                        RaycastHit hit;
                        Physics.Raycast(transform.position, player.transform.position - transform.position, out hit,chaseRange, playerMask);
                        if(hit.transform != null)
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                chasing = true;
                                Chase?.Invoke();
                            }
                        }
                    }
                }
                //if (chasing) { animator.SetBool("moving", false); }
            }
        }
    }

    private void GetStatsFromSo()
    {
        maxHealth = enemyStats.maxHealth;

        chase = enemyStats.chase;
        detectionRange = enemyStats.detectionRange;
        chaseRange = enemyStats.chaseRange;

        run = enemyStats.run;
        runningRange = enemyStats.runningRange;

        range = enemyStats.range;
        projectilePrefab = enemyStats.projectilePrefab;

        rangeToAttack = enemyStats.rangeToAttack;
        attackRange = enemyStats.attackRange;
        attackCooldown = enemyStats.attackCooldown;

        movementSpeed = enemyStats.movementSpeed;
        runSpeed = enemyStats.runSpeed;
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
        }
    }

    public void Die()
    {
        dead = true;
        animator.SetBool("dead", true);
        Destroy(gameObject);
    }

    public void DamagePlayerTouched()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
        //if (hits.Length != 0) hits[0].GetComponent<Player>().TakeDamage();

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
        Vector2 direction = (transform.position - player.transform.position).normalized;

        Transform closestRunningPoint = null;
        float shortestAngle = Mathf.Infinity;
        for(int i = 0; i < runningPointsParent.childCount; i++)
        {
            Transform runningPoint = runningPointsParent.GetChild(i);
            float angle = Vector3.Angle((runningPoint.position - transform.position).normalized, direction);

            if(runningPoint != latestRunningPoint && (closestRunningPoint == null || angle < shortestAngle))
            {
                closestRunningPoint = runningPoint;
                shortestAngle = angle;
            }
        }
        latestRunningPoint = closestRunningPoint;
        return closestRunningPoint.position;
    }
}
