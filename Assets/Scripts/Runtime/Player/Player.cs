using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Vars
    [SerializeField] private Player_Movement controller;

    public enum Character { gyaru, mysterious, officeworker};
    public Character character;

    int health = 3;
    private bool dead;

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
    #region Animator related Actions
    private Action AttackAnimation;
    private Action GetHitAnimation;
    private Action DeathAnimation;
    private Action RunAnimation;
    private Action AbilityAnimation;
    private Action InteractAnimation;
    #endregion
    #endregion

    #region Monobehaviours Methods
    private void Start()
    {
        SwitchCharacter(character);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isMoving)
        {
            RunAnimation?.Invoke();
        }
        if (!stopControlls && !dead)
        {
            if (Input.GetKeyDown(KeyCode.C) && timeToAttack <= Time.timeSinceLevelLoad)
            {
                Debug.Log("Player attacks");
                playerAttackWEvent?.Post(gameObject);
                AttackAnimation?.Invoke();
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
                    if (hit.transform.GetComponent<DoorScript>() != null) { hit.transform.GetComponent<DoorScript>().UseDoor(transform); InteractAnimation?.Invoke(); }
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
                AbilityAnimation?.Invoke();
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
    #endregion

    #region Helath related methods
    public void TakeDamage()
    {
        health--;
        if(health <= 0) { Die(); }
        else
        {
            GetHitAnimation?.Invoke();
        }
    }

    public void OnHit(string hitBy)
    {
        AkSoundEngine.SetSwitch("PlayerHitSwitch", hitBy, gameObject);
        PlayerHitEvent.Post(gameObject);
    }

    private void Die()
    {
        dead = true;
        DeathAnimation?.Invoke();
    }
    #endregion

    #region Switch Character Methods
    public void SwitchCharacter(Character character)
    {
        SetAbilityActionForCharacter(character);
        SetAnimatorsActionsForCharacter(character);
    }

    private void SetAnimatorsActionsForCharacter(Character character)
    {
        switch (character)
        {
            case Character.gyaru:
                AttackAnimation = () => Debug.Log("Gyaru attack animation");
                GetHitAnimation = () => Debug.Log("Gyaru get hit animation");
                DeathAnimation = () => Debug.Log("Gyaru death animation");
                RunAnimation = () => Debug.Log("Gyaru running animation");
                AbilityAnimation = () => Debug.Log("Gyaru ability use animation");
                InteractAnimation = () => Debug.Log("Gyaru interact animation");
                break;
            case Character.mysterious:
                AttackAnimation = () => Debug.Log("mysterious attack animation");
                GetHitAnimation = () => Debug.Log("mysterious get hit animation");
                DeathAnimation = () => Debug.Log("mysterious death animation");
                RunAnimation = () => Debug.Log("mysterious running animation");
                AbilityAnimation = () => Debug.Log("mysterious ability use animation");
                InteractAnimation = () => Debug.Log("mysterious interact animation");
                break;
            case Character.officeworker:
                AttackAnimation = () => Debug.Log("officeworker attack animation");
                GetHitAnimation = () => Debug.Log("officeworker get hit animation");
                DeathAnimation = () => Debug.Log("officeworker death animation");
                RunAnimation = () => Debug.Log("officeworker running animation");
                AbilityAnimation = () => Debug.Log("officeworker ability use animation");
                InteractAnimation = () => Debug.Log("officeworker interact animation");
                break;
        }
    }

    private void SetAbilityActionForCharacter(Character character)
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
    #endregion
}
