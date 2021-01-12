using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenMonster : EnemyBase
{
    public override void Start()
    {
        base.Start();

        attackAnimation = () => {
            float angle = Vector3.Angle(player.transform.forward, transform.forward);
            if(angle < 90f)
            {
                animator.SetInteger("attackType", 2);
            }
            else
            {
                animator.SetInteger("attackType", UnityEngine.Random.Range(0, 2));
            }
            animator.SetTrigger("attack");
        };

        hitAnimation = () => { animator.SetInteger("randomHurt", UnityEngine.Random.Range(0, 3)); animator.SetTrigger("hit"); };

        Attack = DamagePlayerTouched;
        Chase = ChasePlayer;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}