using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    private new Collider collider;
    private Rigidbody rb;
    public TelekinesyGuy enemy;

    private bool isReady;
    private bool isThrowed;
    public float preparationDuration;
    private float preparationTimer;

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event hitWall;

    Vector3 spawnPos;
    Quaternion spawnRot;

    Vector3 posTarget;
    Vector3 dir;

    Vector3 throwTarget;
    Vector3 randomRotationAxis;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        spawnPos = transform.position;
        spawnRot = transform.rotation;

        posTarget = spawnPos + 6f * Vector3.up;
        dir = (posTarget - spawnPos);

        preparationTimer = preparationDuration;

        randomRotationAxis = Vector3.zero;
        while(randomRotationAxis == Vector3.zero) { randomRotationAxis = new Vector3(Random.Range(0, 2f), Random.Range(0, 2f), Random.Range(0, 2f)); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
        {
            preparationTimer -= Time.deltaTime;
            if (preparationTimer < 0)
            {
                transform.position = posTarget;
                transform.rotation = Quaternion.LookRotation(PlayerHelper.instance.transform.position - posTarget);
                isReady = true;
                Invoke("Throw", 0.5f);
            }
            else
            {
                transform.position = Vector3.Lerp(spawnPos, posTarget, 1 - (preparationTimer/preparationDuration));
                transform.rotation = Quaternion.Lerp(spawnRot, Quaternion.LookRotation(PlayerHelper.instance.transform.position - posTarget), 1 - (preparationTimer / preparationDuration));
            }
            enemy.isSpawningSpikes = true;
        }
        else if(!isThrowed)
        {
            transform.Rotate(randomRotationAxis, 360f * 2 * (Time.deltaTime / 0.5f));
            enemy.isSpawningSpikes = true;
        }
        //Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 0.1f);
        //Debug.DrawRay(transform.position, transform.up * 2f, Color.green, 0.1f);

    }

    private void Throw()
    {
        enemy.animator.SetInteger("attackType",Random.Range(0,2));
        enemy.animator.SetTrigger("attack");
        isThrowed = true;
        collider.enabled = true;
        Vector3 throwDir = (PlayerHelper.instance.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(throwDir);
        rb.AddForce(throwDir * 2f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && isReady)
        {
            Player_Movement player = other.gameObject.GetComponent<Player_Movement>();
            if (player != null) player.OnHit("Shard");

            float angle = Vector3.Angle(other.transform.forward, transform.forward);
            if (angle <= 85f) { /* Back stab */}
            else { /* simple damage on player */}

        }
        else if (!other.transform.CompareTag("Enemy")) { Destroy(gameObject); hitWall?.Post(gameObject); }
    }

    private void OnDestroy()
    {
        enemy.isSpawningSpikes = false;
        enemy.agent.isStopped = false;
    }
}

