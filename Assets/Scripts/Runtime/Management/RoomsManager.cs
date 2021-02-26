using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager instance;
    private void Awake(){instance = this;}


    public List<Room> AllRooms = new List<Room>();

    private void Start()
    {
        SpawnEnemiesOfRoom(WorldProgress.instance.locationName);
    }

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
        #region Spawn enemies
    public void SpawnEnemiesOfRoom(int roomInt)
    {
        Room room = AllRooms[roomInt];
        SpawnEnemiesOfRoom(room);
    }
    public void SpawnEnemiesOfRoom(string roomName)
    {
        Room room = GetRoomByName(roomName);
        SpawnEnemiesOfRoom(room);
    }
    private void SpawnEnemiesOfRoom(Room room)
    {
        foreach(EnemyToSpawn enemyToSpawn in room.enemiesToSpawn)
        {
            EnemyBaseAI enemy = Instantiate(enemyToSpawn.enemy, enemyToSpawn.spawnPos, Quaternion.Euler(enemyToSpawn.spawnRot));
            room.enemies.Add(enemy);
        }
    }
        #endregion
        #region Destroy enemies
    public void DestroyEnemiesOfRoom(int roomInt)
    {
        Room room = AllRooms[roomInt];
        DestroyEnemiesOfRoom(room);
    }
    public void DestroyEnemiesOfRoom(string roomName)
    {
        Room room = GetRoomByName(roomName);
        DestroyEnemiesOfRoom(room);
    }
    private void DestroyEnemiesOfRoom(Room room)
    {
        foreach(EnemyBaseAI enemy in room.enemies)
        {
            Destroy(enemy.gameObject);
        }
        room.enemies.Clear();
    }
        #endregion
    #endregion

    public Room GetRoomByName(string roomName)
    {
        foreach(Room room in AllRooms)
        {
            if (room.roomName == roomName) return room;
        }
        return null;
    }

    public void CheckIfRoomNameExists(string roomName)
    {
        foreach (Room room in AllRooms)
        {
            if (roomName == room.roomName) return;
        }
        Debug.LogError("Custom error: Room name doesn't exists in the rooms manager");
    }

}

[Serializable]public class Room
{
    public string roomName;
    public List<EnemyToSpawn> enemiesToSpawn;
    [HideInInspector]
    public List<EnemyBaseAI> enemies = new List<EnemyBaseAI>();

}

[Serializable]public class EnemyToSpawn 
{
    public EnemyBaseAI enemy;
    public Vector3 spawnPos;
    public Vector3 spawnRot;

    public EnemyToSpawn(EnemyBaseAI enemy, Vector3 spawnPos, Vector3 spawnRot)
    {
        this.enemy = enemy;
        this.spawnPos = spawnPos;
        this.spawnRot = spawnRot;
    }
}