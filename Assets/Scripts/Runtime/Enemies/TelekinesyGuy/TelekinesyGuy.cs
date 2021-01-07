using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesyGuy : EnemyBase
{
    [SerializeField] private Vector2 spikeSpawnDurationMinMax;
    private new Collider collider;
    private bool isActive;
    public bool isTrigger;

    public override void Start()
    {
        base.Start();

        Attack = SpawnSpike;
        Chase = ChasePlayer;

        agent.enabled = false;
        rb.useGravity = false;
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    public override void Update()
    {
        if(isActive){
            base.Update();
        }
        else if (isTrigger) { transform.position += Vector3.up * 0.5f * Time.deltaTime; }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            isActive = true;
            agent.enabled = true;
            rb.useGravity = true;
            collider.isTrigger = false;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    private void SpawnSpike() 
    {
        Vector2 temp = UnityEngine.Random.insideUnitCircle * 2.5f;
        Vector3 insideSphere = new Vector3(temp.x, Random.Range(-1f,1.01f), temp.y);
        Vector3 spikeSpawnPos = transform.position + insideSphere - 2f * Vector3.up;


        Quaternion spikeSpawnRot = Quaternion.LookRotation(Vector3.right, -(player.transform.position - transform.position) + new Vector3(Random.Range(-1,1.01f), Random.Range(-1, 1.01f), Random.Range(-1, 1.01f)));


        GameObject go = Instantiate(projectilePrefab, spikeSpawnPos, spikeSpawnRot);
        Spike spike = go.GetComponent<Spike>();
        spike.preparationDuration = Random.Range(spikeSpawnDurationMinMax.x, spikeSpawnDurationMinMax.y);
        spike.enemy = this;
    }
}