using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemyHere : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Enemies = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            {
            Instantiate(Enemies[0], transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Instantiate(Enemies[2], transform.position, Quaternion.identity);
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }
}
