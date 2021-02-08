using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    public Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.OnHit("Shard");

            float angle = Vector3.Angle(other.transform.forward, transform.forward);
            if (angle <= 85f) { /* Back stab */}
            else { /* simple damage on player */}

        }
        else if (!other.transform.CompareTag("Enemy"))
        {
            Destroy(GetComponentInChildren<Renderer>());
            Destroy(gameObject, 4f);
        }
    }
}

