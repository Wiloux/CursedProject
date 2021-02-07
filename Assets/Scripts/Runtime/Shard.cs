using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    private new Collider collider;
    [HideInInspector] public Rigidbody rb;
    public TelekinesyGuy enemy;
    public GameObject PS;

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event hitWall;

    Vector3 spawnPos;
    Quaternion spawnRot;

    Vector3 posTarget;
    Vector3 dir;

    Vector3 throwTarget;
    Vector3 randomRotationAxis;

    private bool isDestroyed;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        spawnPos = transform.position;
        spawnRot = transform.rotation;

        posTarget = spawnPos + 6f * Vector3.up;
        dir = (posTarget - spawnPos);

        randomRotationAxis = Vector3.zero;
        while (randomRotationAxis == Vector3.zero) { randomRotationAxis = new Vector3(Random.Range(0, 2f), Random.Range(0, 2f), Random.Range(0, 2f)); }
    }

    //private IEnumerator Throw(float seconds)
    //{
    //    //for(int i = 0; i < 10; i++)
    //    //{
    //    //    while (isPaused) { Debug.Log("retaining"); yield return null; }
    //    //    Debug.Log(seconds / 10f);
    //    //    yield return new WaitForSecondsRealtime(seconds / 10f);
    //    //}
    //    enemy.animator.SetInteger("attackType", Random.Range(0, 2));
    //    enemy.animator.SetTrigger("attack");
    //    collider.enabled = true;
    //    ImpulseShard();

    //    //StartCoroutine(PlayOnIdleEnterWEventLater(4f));
    //}
    private void ImpulseShard()
    {
        Vector3 throwDir = (PlayerHelper.instance.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(throwDir);
        rb.AddForce(throwDir * 2f, ForceMode.Impulse);
    }

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
            isDestroyed = true;
            Destroy(GetComponentInChildren<Renderer>());
            Destroy(collider);
            Destroy(gameObject, 4f);

            rb.velocity = Vector3.zero;
            hitWall?.Post(gameObject);
            PS.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        enemy.isSpawningSpikes = false;
        enemy.agent.isStopped = false;
    }

    //private IEnumerator PlayOnIdleEnterWEventLater(float time)
    //{
    //    invoking = true;
    //    for (int i = 0; i < 10; i++)
    //    {
    //        while (isPaused) { yield return null; }
    //        yield return new WaitForSeconds(time / 10f);
    //    }
    //    enemy.PlayOnIdleEnterWEvent();
    //    invoking = false;
    //}
}

