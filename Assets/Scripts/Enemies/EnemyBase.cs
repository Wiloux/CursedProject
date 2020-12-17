using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("Nav Vars")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    //[SerializeField] protected Player player;
    [SerializeField] protected GameObject player;

    [Space]
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    private bool dead;

    [Space]
    [Header("Chase")]
    [SerializeField] private bool chase;
    protected bool chasing;
    [SerializeField] protected float detectionRange = 10f;
    private bool detected;
    [SerializeField] protected float chaseRange = 15f;

    [Space]
    [Header("Attack vars")]
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
            chasing = false;
            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
            if(distance < rangeToAttack)
            {
                if(timeToAttack <= Time.timeSinceLevelLoad)
                {
                    // Attack
                    animator.SetTrigger("attack");
                    timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
                    //Attack?.Invoke();
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
                    if (hit.transform.CompareTag("Player"))
                    {
                        //Chase?.Invoke();
                        agent.SetDestination(player.transform.position);
                    }
                }
            }
            if (chasing) { animator.SetBool("moving", false); }
        }
        //else if(distance < detectionRange)
        //{
        //    if (!detected) detected = true;
        //}
        //else if(distance < chaseRange)
        //{
        //    if (chase && detected)
        //    {
        //        Vector2 direction = player.transform.position - transform.position;
        //        direction.y = 0;
        //        direction.Normalize();

        //        rb.MovePosition(rb.position + direction * movementSpeed * Time.deltaTime);
        //    }
        //}
        //else
        //{
        //    if (detected) detected = false;
        //}
    }

    private void OnDrawGizmosSelected()
    {
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

    //public void MeleeAttack()
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerMask);
    //    if (hits.Length != 0) hits[0].GetComponent<Player>().TakeDamage();

    //    if (Vector2.Distance(player.transform.position, transform.position) >= 2.5f) timeToAttack = Time.timeSinceLevelLoad;
    //}
    //public void RangeAttack()
    //{
    //    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    //    Projectile proj = projectile.GetComponent<Projectile>();
    //    proj.direction = (player.transform.position - transform.position).normalized;
    //}
}
