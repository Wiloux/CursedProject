using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Character { gyaru, mysterious, officeworker};
    public Character character;

    public bool stopControlls;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackPointRange;
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private float attackCooldown;
    private float timeToAttack;
    [SerializeField] private float attackRange;

    [SerializeField] private AK.Wwise.Event playerAttackWEvent;
    [SerializeField] private AK.Wwise.Event PlayerHitEvent;

    private Action UseAbility;

    private void Start()
    {
        SwitchCharacter(character);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopControlls)
        {
            if (Input.GetKeyDown(KeyCode.C) && timeToAttack <= Time.timeSinceLevelLoad)
            {
                Debug.Log("Player attacks");
                playerAttackWEvent?.Post(gameObject);
                Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackPointRange, attackLayerMask);
                if(hits.Length > 0)
                {
                    foreach(Collider hit in hits)
                    {
                        if (hit.CompareTag("Enemy"))
                        {
                            EnemyBase enemy = hits[0].GetComponent<EnemyBase>();
                            if (enemy != null) { EnemyHelper.TakeDamage(enemy); Debug.Log("tryingtodamage"); }
                        }
                    }
                }
                timeToAttack = Time.timeSinceLevelLoad + attackCooldown;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, transform.forward, out hit, 5f);
                if (hit.transform != null)
                {
                    if (hit.transform.GetComponent<DoorScript>() != null) hit.transform.GetComponent<DoorScript>().UseDoor(transform);
                    else if (hit.transform.CompareTag("SavePoint"))
                    {
                        GameHandler.instance.TogglePause();
                        GameHandler.instance.ToggleSaveMenu();
                        MouseManagement.instance.ToggleMouseLock();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Player use his ability");
                UseAbility?.Invoke();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackPointRange);
    }

    public void OnHit(string hitBy)
    {
        AkSoundEngine.SetSwitch("PlayerHitSwitch", hitBy, gameObject);
        PlayerHitEvent.Post(gameObject);
    }

    public void SwitchCharacter(Character character)
    {
        switch (character)
        {
            case Character.gyaru:
                UseAbility = () => Debug.Log("Gyaru ability is used");
                break;
            case Character.mysterious:
                UseAbility = () => Debug.Log("Mysterious guy ability is used");
                break;
            case Character.officeworker:
                UseAbility = () => Debug.Log("Office worker ability is used");
                break;
        }
    }
}
