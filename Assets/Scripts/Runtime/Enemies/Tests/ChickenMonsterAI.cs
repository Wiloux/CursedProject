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
    private IUnit unit;
    private Transform player;

    [SerializeField] private EnemyProfileSO enemyProfile;
 //   private 
    private int health;

    private void Awake()
    {
        unit = GetComponent<IUnit>();
        player = PlayerHelper.instance.transform;
        health = enemyProfile.maxHealth;
        state = State.Looking;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
