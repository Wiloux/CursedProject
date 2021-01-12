using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    private void Awake(){instance = this;}


    public List<RoomInfo> AllRooms = new List<RoomInfo>();


    #region Custom Inspector Method
    public void HelpReferencement(int roomInt)
    {
        foreach(EnemyToSpawn enemyToSpawn in AllRooms[roomInt].enemiesToSpawn)
        {
            if(enemyToSpawn.enemy != null)
            {
                enemyToSpawn.spawnPos = enemyToSpawn.enemy.transform.position;
                enemyToSpawn.spawnRot = enemyToSpawn.enemy.transform.eulerAngles;
            }
        }
        Debug.Log("Done");
    }
    #endregion

    #region Enemies Management
    public void SpawnEnemies(int roomInt)
    {
        foreach(EnemyToSpawn enemyToSpawn in AllRooms[roomInt].enemiesToSpawn)
        {
            EnemyBase enemy = Instantiate(enemyToSpawn.enemy, enemyToSpawn.spawnPos, Quaternion.Euler(enemyToSpawn.spawnRot));
            AllRooms[roomInt].enemies.Add(enemy);
        }
    }
    public void DestroyEnemies(int roomInt)
    {
        foreach(EnemyBase enemy in AllRooms[roomInt].enemies)
        {
            Destroy(enemy.gameObject);
        }
        AllRooms[roomInt].enemies.Clear();
    }
    #endregion
}

[Serializable]public class RoomInfo
{

    public string RoomName;
    public List<EnemyToSpawn> enemiesToSpawn;
    [HideInInspector]
    public List<EnemyBase> enemies = new List<EnemyBase>();

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