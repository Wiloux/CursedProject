using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMonsterAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Looking,
        Chasing,
        Attacking,
        Running,
        Watching
    }
    private State state;
    private EnemyBaseUnit unit;
    private Transform player;

    [SerializeField] private EnemyProfileSO enemyProfile;
 //   private 
    private int health;

    private void Awake()
    {
        unit = GetComponent<EnemyBaseUnit>();
        player = PlayerHelper.instance.transform;
        health = enemyProfile.maxHealth;
        state = State.Looking;
    }
    

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Looking:
                unit.LookForPlayer(() => state = State.Chasing);
                break;
            case State.Chasing:
                unit.ChaseThePlayer(0.25f, () => state = State.Attacking);
                break;
            case State.Attacking:
                unit.Attack(() => state = State.Running);
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
