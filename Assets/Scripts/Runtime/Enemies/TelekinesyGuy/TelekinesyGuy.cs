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

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event riseWEvent;
    [SerializeField] private AK.Wwise.Event onIdleEnterWEvent;

    public override void Start()
    {
        base.Start();

        Attack = SpawnSpike;
        Chase = ChasePlayer;

        attackAnimation = () => animator.SetTrigger("summon");

        hitAnimation = () => animator.SetTrigger("hit");
        hitAnimation += () => animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 2));

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
            Invoke(nameof(PlayOnIdleEnterWEvent), 2.4f);

            isActive = true;
            agent.enabled = true;
            rb.useGravity = true;
            collider.isTrigger = false;
            // Set cooldown attack
            attackTimer = attackCooldown;
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
    }

    public void TriggerRise()
    {
        riseWEvent?.Post(gameObject);
        isTrigger = true;
    }

    public void PlayOnIdleEnterWEvent()
    {
        if (!dead) onIdleEnterWEvent?.Post(gameObject);
        Debug.Log("playin entering idle");
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        if(health <= 0)
        {
            Invoke(nameof(PlayOnIdleEnterWEvent), 0.4f);
        }
    }
}