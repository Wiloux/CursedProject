using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseAI : MonoBehaviour
{
    protected enum State
    {
        Idle,
        Looking,
        Chasing,
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

    protected bool attacking;

    #region Animations Actions
    protected Action attackAnimation;
    protected Action hitAnimation;
    #endregion

    public virtual void Awake()
    {
        unit = GetComponent<EnemyUnit>();
        unit.agent = agent;
        unit.rb = rb;
        
        health = enemyProfile.maxHealth;
        state = State.Looking;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        player = PlayerHelper.instance.transform;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (agent.velocity != Vector3.zero) { animator.SetBool("moving", true); }
        else { animator.SetBool("moving", false); }
        
        LaunchActions();
    }

    public virtual void LaunchActions()
    {
        switch (state)
        {
            case State.Looking:
                unit.LookForPlayer(() => state = State.Chasing);
                break;
            case State.Chasing:
                unit.ChaseThePlayer(1f, () => state = State.Attacking);
                break;
            case State.Attacking:
                if (!attacking)
                {
                    attacking = true;
                    unit.Attack(2f, () => { attacking = false; state = State.Chasing; });
                    attackAnimation?.Invoke();
                    //enemyProfile.attackWEvent?.Post(gameObject);
                }
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
            unit.GetStaggered(enemyProfile.staggerDuration, null);
            hitAnimation?.Invoke();
            enemyProfile.getHitWEvent?.Post(gameObject);
        }
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
}
