using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenAI : EnemyBaseAI
{
    private new Collider collider;
    private Vector3 spawnPos;
    [SerializeField] private float spawnDuration;
    private float spawnSpeed;

    private enum ShardState
    {
        None,
        Spawning,
        Rising,
        Rotating,
        Launched
    }
    private ShardState shardState;
    private Shard shard;

    private Vector3 shardSpawnPos;
    private Quaternion shardSpawnRot;
    private Vector3 shardTargetPos;
    private Quaternion shardTargetRot;

    private float timer;
    [SerializeField] private float shardRisingDuration;
    [SerializeField] private float shardRotationDuration;
    private Vector3 shardRotationAxis;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        attackAnimation = () => animator.SetTrigger("summon");

        hitAnimation = () => animator.SetTrigger("hit");
        hitAnimation += () => animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 2));

        agent.enabled = false;
        rb.useGravity = false;
        collider = GetComponent<Collider>();
        //collider.isTrigger = true;
        collider.enabled = false;

        spawnSpeed = 5f / spawnDuration;
        spawnPos = transform.position + transform.up * 5f;

        state = State.Idle;
    }

    public override void LaunchActions()
    {
        switch (state)
        {
            case State.Spawning:
                transform.position += transform.up * spawnSpeed * Time.deltaTime;
                if(transform.position.y > spawnPos.y) {
                    transform.position = spawnPos;
                    collider.enabled = true;
                    rb.useGravity = true;
                    agent.enabled = true;
                    state = State.Chasing; 
                }
                break;
            case State.Chasing:
                unit.ChaseThePlayer(enemyProfile.rangeToAttack, () => state = State.Summoning);
                break;
            case State.Summoning:
                HandleShard();
                break;
            case State.Dead:
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    this.enabled = false;
                    return;
                }
                rb.useGravity = false;
                collider.enabled = false;
                agent.enabled = false;
                transform.position += Vector3.down * spawnSpeed * Time.deltaTime;
                break;
        }
        if (shardState == ShardState.None && state != State.Dead)
        {
            timer -= Time.deltaTime;
            if(timer < 0) shardState = ShardState.Spawning;
        }
    }

    private void HandleShard()
    {
        switch (shardState)
        {
            case ShardState.None:
                if(unit.GetDistanceFromPlayer() > enemyProfile.rangeToAttack)
                {
                    unit.ChaseThePlayer(enemyProfile.rangeToAttack, () => state = State.Summoning);
                }
                break;
            case ShardState.Spawning:
                SpawnSpike();

                timer = shardRisingDuration;
                shardState = ShardState.Rising;
                break;
            case ShardState.Rising:
                // move shard upward
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    shardState = ShardState.Rotating;
                    timer = shardRotationDuration;
                    shard.transform.position = shardTargetPos;
                    shard.transform.rotation = shardTargetRot;
                    while (shardRotationAxis == Vector3.zero) { shardRotationAxis = new Vector3(Random.Range(0, 2f), Random.Range(0, 2f), Random.Range(0, 2f)); }
                    return;
                }
                shard.transform.position = Vector3.Lerp(spawnPos, shardTargetPos, 1 - (timer / shardRisingDuration));
                shard.transform.rotation = Quaternion.Lerp(shardSpawnRot, Quaternion.LookRotation(PlayerHelper.instance.transform.position - shardTargetPos), 1 - (timer / shardRisingDuration));
                break;
            case ShardState.Rotating:
                // rotate shard
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    shardState = ShardState.Launched;
                    shard.transform.rotation = shardTargetRot;
                    return;
                }
                shard.transform.Rotate(shardRotationAxis, 360f * 2 * (Time.deltaTime / 0.5f));
                break;
            case ShardState.Launched:
                // launch shard
                Vector3 throwDir = (PlayerHelper.instance.transform.position - shard.transform.position).normalized;
                shard.rb.AddForce(throwDir * 2f, ForceMode.Impulse);
                shardState = ShardState.None;
                timer = enemyProfile.attackCooldown;
                state = State.Chasing;
                break;
        }
    }

    public void StartSpawn()
    {
        state = State.Spawning;
    }
    private void SpawnSpike()
    {
        //isSpawningSpikes = true;

        Vector2 temp = UnityEngine.Random.insideUnitCircle * 2.5f;
        Vector3 insideSphere = new Vector3(temp.x, Random.Range(-1f, 1.01f), temp.y);
        Vector3 shardSpawnPos = transform.position + insideSphere - 2f * Vector3.up;


        Quaternion shardSpawnRot = Quaternion.LookRotation(Vector3.right, -(player.transform.position - transform.position) + new Vector3(Random.Range(-1, 1.01f), Random.Range(-1, 1.01f), Random.Range(-1, 1.01f)));


        GameObject go = Instantiate(enemyProfile.projectilePrefab, shardSpawnPos, shardSpawnRot);
        this.shardSpawnPos = shardSpawnPos;
        this.shardSpawnRot = shardSpawnRot;
        this.shardTargetPos = shardSpawnPos + Vector3.up * 6f;
        this.shardTargetRot = Quaternion.LookRotation(PlayerHelper.instance.transform.position - shardTargetPos);
        shard = go.GetComponent<Shard>();
        //spike.enemy = this;
    }

    public override void GetStaggered()
    {
        if(shard.gameObject != null)Destroy(shard.gameObject);
        shardState = ShardState.None;
        unit.GetStaggered(enemyProfile.staggerDuration, () => state = State.Summoning);
    }

    public override void Die()
    {
        base.Die();
        timer = spawnDuration;
        Destroy(shard.gameObject);
    }
}
