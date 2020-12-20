using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyAndSpawnPosition> enemiesAndSpawnPos;
    private List<EnemyBase> enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]public class EnemyAndSpawnPosition 
{
    public EnemyBase enemy;
    public Vector3 spawnPos;

    public EnemyAndSpawnPosition(EnemyBase enemy, Vector3 spawnPos)
    {
        this.enemy = enemy;
        this.spawnPos = spawnPos;
    }
}