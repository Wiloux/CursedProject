using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelper
{
    public static void TakeDamage(EnemyBase enemy)
    {
        enemy.TakeDamage();
    }

    public static void TogglePause(EnemyBase enemy)
    {
        if (enemy.pause){Unpause(enemy); }
        else { Pause(enemy); }
    }

    public static void Pause(EnemyBase enemy)
    {
        enemy.animator.enabled = !enemy.animator.isActiveAndEnabled;
        enemy.pause = true;
        enemy.wasAgentStopped = enemy.agent.isStopped;
        enemy.wasAgentDestination = enemy.agent.destination;
        enemy.agent.SetDestination(enemy.transform.position);
        enemy.agent.isStopped = true;
    }

    public static void Unpause(EnemyBase enemy)
    {
        enemy.animator.enabled = !enemy.animator.isActiveAndEnabled;
        enemy.pause = false;
        enemy.agent.isStopped = enemy.wasAgentStopped;
        enemy.agent.SetDestination(enemy.wasAgentDestination);
        Debug.Log(enemy.wasAgentDestination);
        enemy.wasAgentStopped = false;
    }
}
