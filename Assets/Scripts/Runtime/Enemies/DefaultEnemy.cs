using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultEnemy : EnemyBase
{
    void Start()
    {
        Attack = DamagePlayerTouched;
        Chase = ChasePlayer;

    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}