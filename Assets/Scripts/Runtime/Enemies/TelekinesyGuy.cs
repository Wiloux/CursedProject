using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesyGuy : EnemyBase
{
    [SerializeField] private Vector2 spikeSpawnDurationMinMax;
    public override void Start()
    {
        base.Start();

        Attack = SpawnSpike;
        Chase = ChasePlayer;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    private void SpawnSpike() 
    {
        Vector3 spikeSpawnPos = transform.position + UnityEngine.Random.insideUnitSphere - 2f * Vector3.up;
        Quaternion spikeSpawnRot = UnityEngine.Random.rotation;
        GameObject go = Instantiate(projectilePrefab, spikeSpawnPos, spikeSpawnRot);
        go.GetComponent<Spike>().preparationDuration = Random.Range(spikeSpawnDurationMinMax.x, spikeSpawnDurationMinMax.y);
    }
}