using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    public Rigidbody rb;
    [HideInInspector] public float damage;
    public GameObject particle;

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event shardWallHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.OnHit("Shard");

            float angle = Vector3.Angle(other.transform.forward, transform.forward);
            float damage = this.damage;
            if (angle <= 85f) { damage *= 2; }
            PlayerHelper.instance.TakeDamage(damage);
        }
        else if (!other.transform.CompareTag("Enemy"))
        {
            shardWallHit?.Post(gameObject);
            Destroy(GetComponentInChildren<Renderer>());
            Destroy(gameObject, 4f);
            particle.SetActive(true);
        }
    }
}

