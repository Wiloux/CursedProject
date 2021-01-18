using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesyGuy : EnemyBase
{
    [SerializeField] private Vector2 spikeSpawnDurationMinMax;
    private new Collider collider;
    private bool isActive;
    public bool isTrigger;
    public bool isSpawningSpikes;

    public override void Start()
    {
        base.Start();

        Attack = SpawnSpike;
        Chase = ChasePlayer;

        attackAnimation = () => animator.SetTrigger("summon");

        hitAnimation = () => animator.SetTrigger("hit");
        hitAnimation = () => animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 2));

        agent.enabled = false;
        rb.useGravity = false;
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    public override void Update()
    {
        if (isActive)
        {
            base.Update();
        }
        else if (isTrigger) { transform.position += Vector3.up * 1.25f * Time.deltaTime; }
        if (isSpawningSpikes)
        {agent.isStopped = true;}
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            animator.SetTrigger("endSpawn");

            onSpawnWEvent?.Post(gameObject);
            isActive = true;
            agent.enabled = true;
            rb.useGravity = true;
            collider.isTrigger = false;
            // Set cooldown attack
            timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    private void SpawnSpike() 
    {
        isSpawningSpikes = true;

        Vector2 temp = UnityEngine.Random.insideUnitCircle * 2.5f;
        Vector3 insideSphere = new Vector3(temp.x, Random.Range(-1f,1.01f), temp.y);
        Vector3 spikeSpawnPos = transform.position + insideSphere - 2f * Vector3.up;


        Quaternion spikeSpawnRot = Quaternion.LookRotation(Vector3.right, -(player.transform.position - transform.position) + new Vector3(Random.Range(-1,1.01f), Random.Range(-1, 1.01f), Random.Range(-1, 1.01f)));


        GameObject go = Instantiate(projectilePrefab, spikeSpawnPos, spikeSpawnRot);
        Shard spike = go.GetComponent<Shard>();
        spike.enemy = this;
    }
}