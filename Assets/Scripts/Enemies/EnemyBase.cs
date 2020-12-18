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
    //[SerializeField] private Animator animator;
    //[SerializeField] protected Player player;
    [SerializeField] protected GameObject player;

    [Space]
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    private bool dead;

    [Space]
    [Header("Chase")]
    [Tooltip("Does the enemy chase the player")]
    [SerializeField] private bool chase;
    protected bool chasing;
    [SerializeField] protected float detectionRange = 10f;
    private bool detected;
    [SerializeField] protected float chaseRange = 15f;

    [Space]
    [Tooltip("Does the enemy run after hitting the player")]
    [SerializeField] private bool run;
    private bool running;
    [SerializeField] private Transform runningPointsParent;
    private Transform latestRunningPoint;
    [SerializeField] private float runningRange;

    [Space]
    [Header("Attack vars")]
    [Tooltip("Does the enemy attack from distance")]
    [SerializeField] private bool range;
    [SerializeField] protected GameObject projectilePrefab;

    [Space]
    [Tooltip("The range at which the enemy starts to attack")]
    [SerializeField] private float rangeToAttack = 2f;
    [Tooltip("The point where colliders will be detected for the attack")]
    [SerializeField] private Transform attackPoint;
    [Tooltip("Max range between attackPoint and hit colliders")]
    [SerializeField] private float attackRange = 0.5f;
    [Tooltip("Layer Mask used for the colliders detecttion")]
    [SerializeField] private LayerMask playerMask;

    [Space]
    [Tooltip("Cooldown of the attack")]
    [SerializeField] private float attackCooldown;
    private float timeToAttack;

    [SerializeField] private float movementSpeed = 2f;

    protected Action Attack;
    protected Action Chase;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = movementSpeed;
        //player = GameObject.FindObjectOfType<Player>();
        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            float distance = GetDistanceFromPlayer();
            chasing = false;
            //Debug.Log(distance);
            if(run && running)
            {
                if(distance >= runningRange)
                {
                    running = false;
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
                        //animator.SetTrigger("attack");
                        Debug.Log("Attack");
                        timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
                        //Attack?.Invoke();
                        if (run)
                        {
                            running = true;
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
    }

    public void Die()
    {
        dead = true;
        // Death Animation
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
