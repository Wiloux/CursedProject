using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Vars
    [SerializeField] private Player_Movement controller;
    [SerializeField] private Animator animator;

    public enum Character { gyaru, mysterious, officeworker};
    public Character character;

    int health = 3;
    private bool dead;

    public bool stopControlls;

    // Attack vars
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackPointRange;
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private float attackCooldown;
    private float timeToAttack;
    [SerializeField] private float attackRange;

    private bool isChargingAttack;
    [SerializeField] private float chargedAttackCooldown;
    private float timeToChargeAttack;
    [SerializeField] private float attackChargingDuration;
    private float attackChargingTimer;

    [SerializeField] private AK.Wwise.Event playerAttackWEvent;
    [SerializeField] private AK.Wwise.Event PlayerHitEvent;


    private Action UseAbility;
    #region Animator related Actions
    private Action SimpleAttackAnimation;
    private Action ChargingAttackAnimation;
    private Action ChargedAttackAnimation;
    private Action GetHitAnimation;
    private Action DeathAnimation;
    private Action RunAnimation;
    private Action WalkAnimation;
    private Action WalkArmedAnimation;
    private Action AbilityAnimation;
    private Action InteractAnimation;
    #endregion

    #region Wwise Events
    [Header("Wwise Events")]
    [Space(10)]
    [SerializeField] private AK.Wwise.Event WalkRunWSwitch;
    [Header("Charged ATtack")] 
    [SerializeField] private AK.Wwise.Event startChargingAttackWEvent;
    [SerializeField] private AK.Wwise.Event chargedAttackWEvent;
    [SerializeField] private AK.Wwise.Event simpleAttackWEvent;
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
        WalkAnimation?.Invoke();
        //if (controller.isMoving)
        //{
        //}
        if (!stopControlls && !dead)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(ChargeOrAttack());
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

            if (timeToAttack >= 0) timeToAttack -= Time.deltaTime;
            if (timeToChargeAttack >= 0) timeToChargeAttack -= Time.deltaTime;
            if (isChargingAttack)
            {
                if (attackChargingTimer >= 0) attackChargingTimer -= Time.deltaTime;
                else { isChargingAttack = false; timeToChargeAttack = chargedAttackCooldown; ChargedAttackAnimation?.Invoke(); chargedAttackWEvent?.Post(gameObject); }
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

    #region Attack Methods
    private IEnumerator ChargeOrAttack()
    {
        yield return new WaitForSeconds(0.3f);
        if (Input.GetKey(KeyCode.C))
        {
            if(timeToChargeAttack < 0)
            {
                isChargingAttack = true;
                attackChargingTimer = attackChargingDuration;
                ChargingAttackAnimation?.Invoke();
                Debug.Log("charged attack");

            }
        }
        else if (timeToAttack < 0)
        {
            timeToAttack = attackCooldown; SimpleAttackAnimation?.Invoke(); simpleAttackWEvent?.Post(gameObject); Debug.Log("simple attack");

        } // simple attack method attached to the animation
    }
    private void SimpleAttack()
    {
        EnemyBase enemy = GetEnemyToAttack();
        if (enemy == null) return;

        EnemyHelper.TakeDamage(enemy); 
        Debug.Log("Simple attack");
    }
    private void ChargedAttack()
    {
        EnemyBase enemy = GetEnemyToAttack();
        if (enemy == null) return;

        // Do more damage to the enemy
        EnemyHelper.TakeDamage(enemy); // temp
        Debug.Log("Charged attack");

    }

    private EnemyBase GetEnemyToAttack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackPointRange, attackLayerMask);
        if (hits.Length > 0)
        {
            for(int i = 0;i < hits.Length; i++)
            {
                Collider hit = hits[i];
                if (hit.CompareTag("Enemy"))
                {
                    EnemyBase enemy = hits[0].GetComponent<EnemyBase>();
                    if (enemy != null) { return enemy; }
                }
            }
        }
        return null;
    }
    #endregion

    #region Health related methods
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
                SimpleAttackAnimation = () => { animator.SetTrigger("Attack"); animator.SetInteger("AttackAnim", UnityEngine.Random.Range(1, 4)); Debug.Log("Gyaru attack animation"); };
                ChargedAttackAnimation = () => { animator.SetTrigger("Attack"); animator.SetInteger("AttackAnim", 0); Debug.Log("Gyaru big attack animation"); };
                GetHitAnimation = () => { animator.SetTrigger("Hurt"); animator.SetInteger("HurtAnim", UnityEngine.Random.Range(1, 4)); Debug.Log("Gyaru get hit animation"); };
                DeathAnimation = () => { animator.SetTrigger("Hurt"); animator.SetFloat("HP", -1); Debug.Log("Gyaru death animation"); };
                RunAnimation = () => { animator.SetBool("isMoving", true); animator.SetBool("isRunning", true); Debug.Log("Gyaru running animation"); };
                AbilityAnimation = () => Debug.Log("Gyaru ability use animation");
                InteractAnimation = () => { animator.SetTrigger("Action"); Debug.Log("Gyaru interact animation"); };
                WalkAnimation = () => { animator.SetBool("isMoving", true); animator.SetBool("isRunning", false); animator.SetBool("isArmed", false); Debug.Log("Gyaru walking animation"); };
                WalkArmedAnimation = () => { animator.SetBool("isMoving", true); animator.SetBool("isRunning", false); animator.SetBool("isArmed", true); Debug.Log("Gyaru walk armed animation"); };
                break;
            case Character.mysterious:
                SimpleAttackAnimation = () => Debug.Log("mysterious attack animation");
                ChargedAttackAnimation = () => Debug.Log("mysterious big attack animation");
                GetHitAnimation = () => Debug.Log("mysterious get hit animation");
                DeathAnimation = () => Debug.Log("mysterious death animation");
                RunAnimation = () => Debug.Log("mysterious running animation");
                AbilityAnimation = () => Debug.Log("mysterious ability use animation");
                InteractAnimation = () => Debug.Log("mysterious interact animation");
                WalkAnimation = () => { Debug.Log("mysterious walking animation"); };
                WalkArmedAnimation = () => { Debug.Log("mysterious walk armed animation"); };
                break;
            case Character.officeworker:
                SimpleAttackAnimation = () => Debug.Log("officeworker attack animation");
                ChargedAttackAnimation = () => Debug.Log("officeworker big attack animation");
                GetHitAnimation = () => Debug.Log("officeworker get hit animation");
                DeathAnimation = () => Debug.Log("officeworker death animation");
                RunAnimation = () => Debug.Log("officeworker running animation");
                AbilityAnimation = () => Debug.Log("officeworker ability use animation");
                InteractAnimation = () => Debug.Log("officeworker interact animation");
                WalkAnimation = () => { Debug.Log("officeworker walking animation"); };
                WalkArmedAnimation = () => { Debug.Log("officeworker walk armed animation"); };
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
