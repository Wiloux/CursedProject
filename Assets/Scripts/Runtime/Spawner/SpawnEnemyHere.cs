using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyHere : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Enemy;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            {
            Instantiate(Enemy, transform.position, Quaternion.identity);
        }
    }
}
