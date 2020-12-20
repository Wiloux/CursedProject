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
            TransitionManager.instance.StartFade(Using,otherDoorScript.SpawnPoint, 1, otherDoorScript, this);
    }
}
 