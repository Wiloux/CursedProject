using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    bool IsIdle();

    void ChaseThePlayer(Vector3 position, float stopDistance, Action onArrivedAtPosition);

    void LookForPlayer(Action onPlayerFound);

    void RunFromPlayer(float stopDistance, Action onStoppedRunning);

    void Attack(Action afterAttack);

    void WatchThePlayer(Action afterWatching);
}
