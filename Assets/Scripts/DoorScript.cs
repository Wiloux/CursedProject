using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public DoorScript otherDoorScript;
    public Transform SpawnPoint;
    public RoomManager roomManager;


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
        if(roomManager != null) { roomManager.DestroyEnemies(); }
        #endregion

        TransitionManager.instance.StartFade(Using, otherDoorScript.SpawnPoint, 1, otherDoorScript, this);

        #region Spawn enemies of the new room
        RoomManager _roomManager = otherDoorScript.roomManager;
        if(_roomManager!= null){_roomManager.SpawnEnemies();}
        #endregion
    }
}
 