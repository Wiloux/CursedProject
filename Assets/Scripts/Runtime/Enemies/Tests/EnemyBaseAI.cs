using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseAI : MonoBehaviour
{
    protected enum State
    {
        Spawning,
        Idle,
        Looking,
        Chasing,
        Summoning,
        Attacking,
        Running,
        Watching,
        Stagger,
        Dead
    }
    protected State state;
    protected EnemyUnit unit;
    protected Transform player;

    [SerializeField] protected EnemyProfileSO enemyProfile;
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;
    protected int health;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerMask;

    protected bool attacking;
    protected float attackTimer;

    #region Animations Actions
    protected Action attackAnimation;
    protected Action hitAnimation;
    #endregion

    public virtual void Awake()
    {
        unit = GetComponent<EnemyUnit>();
        unit.enemyProfile = enemyProfile;
        unit.agent = agent;
        unit.rb = rb;
        unit.attackPoint = attackPoint;
        unit.playerMask = playerMask;

        health = enemyProfile.maxHealth;
        state = State.Looking;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        player = PlayerHelper.instance.transform;
        enemyProfile.onSpawnWEvent?.Post(gameObject);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(state != State.Dead)
        {
            if (agent.velocity != Vector3.zero) { animator.SetBool("moving", true); }
            else { animator.SetBool("moving", false); }

            LaunchActions();
        }
        Debug.Log(state.ToString());
    }

    public virtual void LaunchActions()
    {
        switch (state)
        {
            case State.Looking:
                unit.LookForPlayer(() => state = State.Chasing);
                break;
            case State.Chasing:
                unit.ChaseThePlayer(enemyProfile.rangeToAttack, () => state = State.Attacking);
                break;
            case State.Attacking:
                if(unit.GetDistanceFromPlayer() <= enemyProfile.rangeToAttack)
                {
                    if (!unit.attacking)
                    {
                        unit.Attack(2f, () => { state = State.Chasing; });
                        attackAnimation?.Invoke();
                        //enemyProfile.attackWEvent?.Post(gameObject);
                    }
                }
                else { state = State.Chasing; }
                break;
        }
    }

    public virtual void TakeDamage()
    {
        Debug.Log(gameObject.name + " took damage");
        health--;
        if (health <= 0) { Die(); }
        else
        {
            GetStaggered();
            state = State.Stagger;
            hitAnimation?.Invoke();
            enemyProfile.getHitWEvent?.Post(gameObject);
        }
    }

    public virtual void GetStaggered()
    {
        unit.GetStaggered(enemyProfile.staggerDuration, () => state = State.Chasing);
    }

    public virtual void Die()
    {
        state = State.Dead;
        unit.Die();

        //agent.SetDestination(transform.position);
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;

        animator.SetTrigger("dead");
        enemyProfile.deathWEvent?.Post(gameObject);
        //Destroy(gameObject);
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
                float damage = enemyProfile.attackDamage;
                if (enemyProfile.backstab)
                {
                    float angle = Vector3.Angle(hits[0].transform.forward, transform.forward);
                    Debug.Log(angle);

                    if (angle < 90f) { damage *= 2; }
                }
                PlayerHelper.instance.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("No gameobject touched with Player layer");
        }

        if (unit.GetDistanceFromPlayer() >= 2.5f) unit.attackTimer = enemyProfile.attackCooldown;
    }
}
