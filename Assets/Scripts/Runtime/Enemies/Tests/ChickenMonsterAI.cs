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
                unit.ChaseThePlayer(enemyProfile.rangeToAttack, () => state = State.Attacking);
                break;
            case State.Attacking:
                //Debug.Log(unit.GetDistanceFromPlayer() + " || " + enemyProfile.rangeToAttack);
                if (unit.GetDistanceFromPlayer() <= enemyProfile.rangeToAttack)
                {
                    if (!unit.attacking && unit.attackTimer < 0)
                    {
                        unit.Attack(2f, () => { state = State.Running; });
                        attackAnimation?.Invoke();
                        //enemyProfile.attackWEvent?.Post(gameObject);
                    }
                }
                else if(!unit.attacking){ state = State.Chasing; }
                break;
            case State.Running:
                unit.RunFromPlayer(0.25f, () => state = State.Watching);
                break;
            case State.Watching:
                unit.WatchThePlayer(() => state = State.Chasing);
                break;
        }
    }

    public override void GetStaggered()
    {
        unit.GetStaggered(enemyProfile.staggerDuration, () => state = State.Running);
    }
}
