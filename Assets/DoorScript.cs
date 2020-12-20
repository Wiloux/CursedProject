using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public DoorScript otherDoorScript;
    public Transform SpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = transform.Find("Spawn").transform;
        if(otherDoorScript != null)
        {
            otherDoorScript.otherDoorScript = this;
        }
    }

    public void UseDoor(Transform Using)
    {
        #region Destroy the enemies of the current room
        RoomSpawner roomSpawner = GetComponent<RoomSpawner>();
        if(roomSpawner != null) { roomSpawner.DestroyEnemies(); }
        #endregion

        TransitionManager.instance.StartFade(Using, otherDoorScript.SpawnPoint, 1, otherDoorScript, this);

        #region Spawn enemies of the new room
        roomSpawner = null;
        roomSpawner = otherDoorScript.GetComponent<RoomSpawner>();
        if(roomSpawner != null){roomSpawner.SpawnEnemies();}
        #endregion
    }
}
 