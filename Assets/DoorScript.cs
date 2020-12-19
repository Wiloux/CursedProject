using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public DoorScript otherDoorScript;
    public GameObject Player;
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

    // Update is called once per frame
    void Update()
    {
        if (Player != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("moved");
            Player.transform.position = new Vector3(otherDoorScript.SpawnPoint.position.x,(transform.position.y -otherDoorScript.SpawnPoint.position.y) + otherDoorScript.SpawnPoint.position.y, otherDoorScript.SpawnPoint.position.z);
            otherDoorScript.Player = Player;
            Player = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Player = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Player = null;
    }
}
