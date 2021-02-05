using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMonsterAI : EnemyBaseAI
{
    // Update is called once per frame
    public override void Start()
    {
        base.Start();

        attackAnimation = () => {
            float angle = Vector3.Angle(player.transform.forward, transform.forward);
            if (angle < 90f)
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
    }

    public override void LaunchActions()
    {
        switch (state)
        {
            case State.Looking:
                unit.LookForPlayer(() => state = State.Chasing);
                break;
            case State.Chasing:
                unit.ChaseThePlayer(1f, () => state = State.Attacking);
                break;
            case State.Attacking:
                if (!attacking)
                {
                    attacking = true;
                    unit.Attack(2f, () => { attacking = false; state = State.Chasing; });
                    attackAnimation?.Invoke();
                    //enemyProfile.attackWEvent?.Post(gameObject);
                }
                break;
            case State.Running:
                unit.RunFromPlayer(0.25f, () => state = State.Watching);
                break;
            case State.Watching:
                unit.WatchThePlayer(() => state = State.Chasing);
                break;
        }
    }
}
