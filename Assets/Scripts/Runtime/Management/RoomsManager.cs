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
        SpawnEnemiesOfRoom(WorldProgressSaver.instance.locationName);
        ChangeRTCPReverbForRoom(WorldProgressSaver.instance.locationName);
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
        if (room != null &&room.enemiesToSpawn.Count != 0)
        {
            foreach (EnemyToSpawn enemyToSpawn in room.enemiesToSpawn)
            {
                EnemyBaseAI enemy = Instantiate(enemyToSpawn.enemy, enemyToSpawn.spawnPos, Quaternion.Euler(enemyToSpawn.spawnRot));
                room.enemies.Add(enemy);
            }
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
        Debug.Log(room.roomName);
            foreach (EnemyBaseAI enemy in room.enemies)
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

    public string GetRoomName(int index) 
    {
        if (index > AllRooms.Count - 1) { Debug.LogError("Custom error: index is too high"); return ""; } 
        else return AllRooms[index].roomName; 
    }

    #region Check room existence
    public void CheckIfRoomNameExists(string roomName)
    {
        foreach (Room room in AllRooms)
        {
            if (roomName == room.roomName) return;
        }
        Debug.LogError("Custom error: Room name : " + roomName + " doesn't exists in the rooms manager");
    }
    public bool DoesRoomExists(string roomName)
    {
        foreach (Room room in AllRooms)
        {
            if (roomName == room.roomName) return true;
        }
        return false;
    }
    #endregion

    public void ChangeRTCPReverbForRoom(string roomName)
    {
        if (!DoesRoomExists(roomName)) return;

        if (roomName == "School hallways" || roomName == "Corridor2ndFloor")
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb", 1);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 0);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_Bathroom", 0);
        }
        else if (roomName == "Girls bathroom")
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_Bathroom", 1);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb", 0);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 0);
        }
        else
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 1);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb", 0);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_Bathroom", 0);
        }
    }
}

[Serializable] public class Room
{
    public string roomName;

    public enum PlaceType { school, city, forest};
    public PlaceType place;


    public List<EnemyToSpawn> enemiesToSpawn;
    [HideInInspector]
    public List<EnemyBaseAI> enemies = new List<EnemyBaseAI>();

}

[Serializable] public class EnemyToSpawn 
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