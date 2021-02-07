using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            transform.parent.GetComponent<WardenAI>().StartSpawn();
            Destroy(gameObject);
        }
    }
}