using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenMonster : EnemyBase
{
    public override void Start()
    {
        base.Start();

        attackAnimation = () => animator.SetInteger("attackType", UnityEngine.Random.Range(0, 2));
        attackAnimation += () => animator.SetTrigger("attack");

        backstabAnimation = () => animator.SetInteger("attackType", 2);
        backstabAnimation += () => animator.SetTrigger("attack");

        hitAnimation = () => animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 3));
        hitAnimation += () => animator.SetTrigger("hit");

        Attack = DamagePlayerTouched;
        Chase = ChasePlayer;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}