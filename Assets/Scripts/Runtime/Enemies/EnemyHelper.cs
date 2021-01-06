using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelper
{
    public static void TakeDamage(EnemyBase enemy)
    {
        enemy.TakeDamage();
    }

    public static void Pause(EnemyBase enemy)
    {
        enemy.animator.enabled = !enemy.animator.isActiveAndEnabled;
        enemy.pause = !enemy.pause;
        enemy.agent.isStopped = !enemy.agent.isStopped;
    }
}
