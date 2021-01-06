using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenMonster : EnemyBase
{
    public override void Start()
    {
        base.Start();

        Attack = DamagePlayerTouched;
        Chase = ChasePlayer;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}