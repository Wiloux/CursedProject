using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomManager : MonoBehaviour
{
    public List<EnemyToSpawn> enemiesToSpawn;
    private List<EnemyBase> enemies = new List<EnemyBase>();

    #region Custom Inspector Method
    public void HelpReferencement()
    {
        foreach(EnemyToSpawn enemyToSpawn in enemiesToSpawn)
        {
            if(enemyToSpawn.enemy != null)
            {
                enemyToSpawn.spawnPos = enemyToSpawn.enemy.transform.position;
                enemyToSpawn.spawnRot = enemyToSpawn.enemy.transform.eulerAngles;
            }
        }
    }
    #endregion

    #region Enemies Management
    public void SpawnEnemies()
    {
        foreach(EnemyToSpawn enemyToSpawn in enemiesToSpawn)
        {
            EnemyBase enemy = Instantiate(enemyToSpawn.enemy, enemyToSpawn.spawnPos, Quaternion.Euler(enemyToSpawn.spawnRot));
            enemies.Add(enemy);
        }
    }
    public void DestroyEnemies()
    {
        foreach(EnemyBase enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }
    #endregion
}

[Serializable]public class EnemyToSpawn 
{
    public EnemyBase enemy;
    public Vector3 spawnPos;
    public Vector3 spawnRot;

    public EnemyToSpawn(EnemyBase enemy, Vector3 spawnPos, Vector3 spawnRot)
    {
        this.enemy = enemy;
        this.spawnPos = spawnPos;
        this.spawnRot = spawnRot;
    }
}