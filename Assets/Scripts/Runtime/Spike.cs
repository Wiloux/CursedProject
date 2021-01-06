using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private new Collider collider;
    private Rigidbody rb;

    private bool isReady;
    public float preparationDuration;
    private float preparationTimer;

    Vector3 spawnPos;
    Quaternion spawnRot;

    Vector3 posTarget;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        isReady = false;

        spawnPos = transform.position;
        spawnRot = transform.rotation;

        posTarget = spawnPos + 3f * Vector3.up;
        dir = (posTarget - spawnPos);

        preparationTimer = preparationDuration;
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
                transform.rotation = Quaternion.LookRotation(Vector3.up, PlayerHelper.instance.transform.position - posTarget);
                isReady = true;
                Invoke("Throw", 0.5f);
            }
            else
            {
                transform.position += (dir / preparationDuration) * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(spawnRot, Quaternion.LookRotation(Vector3.up, PlayerHelper.instance.transform.position - posTarget), 1 - (preparationTimer / preparationDuration));
            }
        }
        else { transform.rotation}
        //Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 0.1f);
        //Debug.DrawRay(transform.position, transform.up * 2f, Color.green, 0.1f);

    }

    private void Throw()
    {
        collider.enabled = true;
        Vector3 throwDir = (PlayerHelper.instance.transform.position - transform.position).normalized;
        rb.AddForce(throwDir * 100);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;
    }
}

