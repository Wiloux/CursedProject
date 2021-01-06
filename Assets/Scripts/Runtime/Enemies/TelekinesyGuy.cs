using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesyGuy : EnemyBase
{
    public override void Start()
    {
        base.Start();

        Attack = StartThrowingSpike;
        Chase = ChasePlayer;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    private void StartThrowingSpike() { StartCoroutine(ThrowSpike()); }

    private IEnumerator ThrowSpike()
    {
        Vector3 spikeSpawnPos = transform.position + UnityEngine.Random.insideUnitSphere - 2 * Vector3.down;
        Quaternion spikeSpawnRot = UnityEngine.Random.rotation;
        GameObject spike = Instantiate(projectilePrefab, spikeSpawnPos, spikeSpawnRot);

        Vector3 spikeTargetPos = spikeSpawnPos + 2.5f * Vector3.up;
        Vector3 spikeDir = (spikeTargetPos - spikeSpawnPos).normalized;
        Quaternion spikeTargetRot = Quaternion.LookRotation(player.transform.position - spikeTargetPos, Vector3.up);

        //yield return new WaitForSeconds(2f);

        float flightDuration = 1f;
        while (true)
        {
            flightDuration -= Time.deltaTime;
            if(flightDuration < 0)
            {
                spike.transform.position = spikeTargetPos;
                spike.transform.rotation = spikeTargetRot;
                break;
            }
            spike.transform.Translate(spikeDir * (1 / (Time.deltaTime * flightDuration)));
            Quaternion.Lerp(spikeSpawnRot, spikeTargetRot, 1 - flightDuration);
            yield return null;
        }
        agent.isStopped = false;

    }
}
