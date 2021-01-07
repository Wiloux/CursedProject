using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesyGuyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            transform.parent.GetComponent<TelekinesyGuy>().isTrigger = true;
            Destroy(gameObject);
        }
    }
}
