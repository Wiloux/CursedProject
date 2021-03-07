using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    #region Vars
    [SerializeField] private Player_Movement controller;
    [SerializeField] private Animator animator;

    public enum Character { gyaru, mysterious, officeworker };
    public Character character;

    public float health = 3;
    public float healthMax = 3;
    private bool dead;

    private Inventory inventory;

    public bool stopControlls;

    // Attack vars
    [SerializeField] private Transform attackWeapon;
    [SerializeField] private float attackPointRange;
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private float attackCooldown;
    private float timeToAttack;

    [SerializeField] private ObjectDuration[] idleBreaksObjects;

    [SerializeField] private float secondaryAttackCooldown;
    private float timeToSecondaryAttack;

    private bool isArmed;
    [SerializeField] private float beingArmedDuration;
    private float unarmTimer;

    private bool isIdle;
    [SerializeField] private Vector2 idleBreakTimerMinMax;
    private float idleTimer;

    private Action UseAbility;
    [SerializeField] private KeyCode specialAbilityKey = KeyCode.F;
    [SerializeField] private float abilityCooldown;
    private float abilityTimer;

    [Space(5)]
    [SerializeField] private KeyCode healKey = KeyCode.H;

    #region Animator related Actions
    private Action SimpleAttackAnimation;
    private Action SecondaryAttackAnimation;
    private Action GetHitAnimation;
    private Action DeathAnimation;
    private Action WalkAnimation;
    private Action WalkBackwardsAnimation;
    private Action StopWalkingAnimation;
    private Action RunAnimation;
    private Action AbilityAnimation;
    private Action InteractAnimation;
    private Action IdleBreakAnimation;
    #endregion

    #region Wwise Events
    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event playerAttackWEvent;
    [SerializeField] private AK.Wwise.Event PlayerHitEvent;
    [Space(10)]
    [SerializeField] private AK.Wwise.Event WalkRunWSwitch;
    [Header("Charged Attack")] 
    //[SerializeField] private AK.Wwise.Event startChargingAttackWEvent;
    [SerializeField] private AK.Wwise.Event secondaryAttackWEvent;
    [SerializeField] private AK.Wwise.Event simpleAttackWEvent;
    #endregion

    #region VFX
    [Header("VFX")]
    [SerializeField] private GameObject attackWeaponParticles;
    [SerializeField] private GameObject weaponImpactParticles;
    [SerializeField] private GameObject bloodParticlesPrefab;
    #endregion
    #endregion

    #region Monobehaviours Methods
    private void Start()
    {
        inventory = GetComponent<Inventory>();
        SwitchCharacter(character);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopControlls && !dead && !GameHandler.instance.IsPaused())
        {
            if (controller.isMoving) 
            { 
                if(controller.isRunning){ SetRunningSound(); RunAnimation?.Invoke();}
                else 
                {
                    if (!controller.isMovingBackwards)
                    {
                        SetWalkingSound();
                        WalkAnimation?.Invoke();
                    }
                    else
                    {
                        SetWalkingBackwardSound();
                        WalkBackwardsAnimation?.Invoke();
                    }
                }
            }
            else { StopWalkingAnimation?.Invoke(); if (!isIdle) { isIdle = true; ResetIdleTimer(); } }

            if (controller.canMove)
            {
                if (Input.GetMouseButtonDown(0) && timeToAttack < 0)
                {
                    // Cooldown gestion
                    timeToAttack = attackCooldown;
                    StartCoroutine(BlockMovementForPeriod(1.5f));
                    // Animation
                    SimpleAttackAnimation?.Invoke();
                    ArmPlayer();
                    // Sound
                    //simpleAttackWEvent?.Post(gameObject);  // DONE BY ANIMATION

                    //controller.canRotate = false;
                    Debug.Log("starting simple attack");
                }
                else if (Input.GetMouseButtonDown(1) && timeToSecondaryAttack < 0 && timeToAttack < 0)
                {
                    // Cooldown gestion
                    timeToSecondaryAttack = secondaryAttackCooldown;
                    timeToAttack = attackCooldown;
                    StartCoroutine(BlockMovementForPeriod(3f));
                    // Animation
                    SecondaryAttackAnimation?.Invoke();
                    ArmPlayer();
                    // Sound
                    //secondaryAttackWEvent?.Post(gameObject); // DONE BY ANIMATION

                    Debug.Log("starting charged attack");
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("e");
                    RaycastHit hit;
                    LayerMask ignoreOcclusioneRaycastCheck = 1<<14;
                    Physics.Raycast(transform.position, transform.forward, out hit, 5f, ~ignoreOcclusioneRaycastCheck);
                    if (hit.transform != null)
                    {
                        Debug.Log(hit.transform);
                        if (hit.transform.GetComponent<DoorScript>() != null) {/* InteractAnimation?.Invoke();*/  hit.transform.GetComponent<DoorScript>().TryUseDoor(transform);  }
                        else if(hit.transform.GetComponent<Collectible>() != null) { InteractAnimation?.Invoke(); Collect(hit.transform.GetComponent<Collectible>()); Destroy(hit.transform.gameObject); }
                        else if (hit.transform.CompareTag("SavePoint"))
                        {
                            GameHandler.instance.TogglePause();
                            GameHandler.instance.ToggleSaveMenu();
                            MouseManagement.instance.ToggleMouseLock();
                        }
                    }
                }
                else if (Input.GetKey(specialAbilityKey))
                {
                    if(abilityTimer < 0)
                    {
                        Debug.Log("Player use his ability");
                        AbilityAnimation?.Invoke();
                        UseAbility?.Invoke();
                        abilityTimer = abilityCooldown;
                    }
                }
                else if (Input.GetKey(healKey))
                {
                    if (inventory.healingItem > 0 && health < healthMax)
                    {
                        // Heal Player
                        Heal();

                        // remove a stack from healing items
                        inventory.RemoveHealingItem();
                    }
                }
            }

            if (timeToAttack >= 0) timeToAttack -= Time.deltaTime;
            if (timeToSecondaryAttack >= 0) timeToSecondaryAttack -= Time.deltaTime;
            if (abilityTimer >= 0) abilityTimer -= Time.deltaTime;
            if (isArmed)
            {
                if (unarmTimer > 0) unarmTimer -= Time.deltaTime;
                else
                {
                    UnarmPlayer();
                }
            }
            if (isIdle)
            {
                if (Input.anyKey || controller.isRotating) isIdle = false;
                else
                {
                    if (idleTimer > 0) idleTimer -= Time.deltaTime;
                    else { IdleBreakAnimation?.Invoke(); ResetIdleTimer(); }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackWeapon == null) return;
        Gizmos.DrawWireSphere(attackWeapon.position, attackPointRange);
    }
    #endregion

    #region Attack Methods
    public void SimpleAttack() // CALLED BY ANIMATION
    {
        EnemyBaseAI enemy = GetEnemyToAttack();
        if (enemy == null)
        {
            Debug.Log("No enemy has been found");
            return;
        }
        Debug.Log("hrgrhogr");

        ToggleImpactParticles();
        EnemyHelper.TakeDamage(enemy); 
        Debug.Log("Simple attack consideration");
    }
    public void SecondaryAttack() // CALLED BY ANIMATION
    {
        EnemyBaseAI enemy = GetEnemyToAttack();
        if (enemy == null)
        {
            Debug.Log("No enemy has been found");
            return; }

        ToggleImpactParticles();
        // Do more damage to the enemy
        EnemyHelper.TakeDamage(enemy); // temp
        Debug.Log("Charged attack consideration");

    }

    private EnemyBaseAI GetEnemyToAttack()
    {
        Collider[] hits = Physics.OverlapSphere(attackWeapon.position, attackPointRange, attackLayerMask);
        if (hits.Length > 0)
        {
            for(int i = 0;i < hits.Length; i++)
            {
                Collider hit = hits[i];
                //Debug.Log(hit.name);

                if (hit.CompareTag("Enemy"))
                {
                    EnemyBaseAI enemy = hits[i].GetComponent<EnemyBaseAI>();
                    if (enemy != null){return enemy; }
                }
                //Debug.Log("---------------");
            }
        }
        return null;
    }
    #endregion

    #region Arm / Unarm Methods
    private void ArmPlayer()
    {
        attackWeapon.gameObject.SetActive(true);
        if (animator == null) return;
        animator.SetBool("isArmed", true);
        isArmed = true;
        unarmTimer = beingArmedDuration;
        Debug.Log("player is armed now");
    }
    private void UnarmPlayer()
    {
        attackWeapon.gameObject.SetActive(false);
        if (animator == null) return;
        animator.SetBool("isArmed", false);
        isArmed = false;
        Debug.Log("player is unarmed now");
    }
    #endregion

    #region Idle Breaks Objects gestion methods

    private void ResetIdleTimer() { idleTimer = UnityEngine.Random.Range(idleBreakTimerMinMax.x, idleBreakTimerMinMax.y); }
    private void PutAndRemoveIdleBreakObject(int index)
    {
        PutIdleBreakObject(index);
        StartCoroutine(RemoveIdleBreakObject(index, idleBreaksObjects[index].duration));
    }
    private void PutIdleBreakObject(int index)
    {
        idleBreaksObjects[index].go.SetActive(true);
    }
    private IEnumerator RemoveIdleBreakObject(int index, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        idleBreaksObjects[index].go.SetActive(false);
    }
    #endregion

    private void Collect(Collectible collectible)
    {
        if (collectible.healingItem)
        {
            inventory.healingItem += collectible.healValues;
            GameHandler.instance.DisplayPickupItemMessage("Healing Item", 3f);
        }
        else
        {
            GameHandler.instance.DisplayPickupItemMessage(collectible.so.name, 3f); 
            inventory.AddObjectToInv(collectible.so);
        }
    }

    private IEnumerator BlockMovementForPeriod(float seconds)
    {
        controller.canMove = false;
        for(int i = 0; i <10; i++)
        {
            // while(isPaused) return yield null;
            yield return new WaitForSeconds(seconds / 10f);
        }
        controller.canMove = true;
    }

    #region Weapon particles toggle methods
    public void EnableAttackWeaponParticles() { attackWeaponParticles.SetActive(true); }
    public void DisableAttackWeaponParticles() { attackWeaponParticles.SetActive(false); }

    private void ToggleImpactParticles() { GameObject impactParticles = Instantiate(weaponImpactParticles, attackWeapon.position, Quaternion.identity); Destroy(impactParticles, 1.5f); }
    #endregion

    #region Health related methods
        #region Heal
    public void Heal() { if (health < healthMax) health++; }
        #endregion
    #region Take Damage
    public void TakeDamage(float damage, bool stagger)
    {
        health -= damage;
        DisableAttackWeaponParticles();
        if (isIdle) ResetIdleTimer();
        if(health <= 0) { Die(); }
        else
        {
            if(stagger) GetHitAnimation?.Invoke();
        }
    }

    public void OnHit(string hitBy)
    {
        AkSoundEngine.SetSwitch("PlayerHitSwitch", hitBy, gameObject);
        PlayerHitEvent.Post(gameObject);
        GameObject bloodParticles = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(bloodParticles, 2.5f);
    }
        #endregion

    public void Die()
    {
        if (!dead)
        {
            dead = true;
            controller.dead = true;
            DeathAnimation?.Invoke();
        }
    }
    #endregion

    #region Wwise swicthes methods
    private void SetWalkingSound() { AkSoundEngine.SetSwitch("WalkRun", "Walk", gameObject);}
    private void SetWalkingBackwardSound() { AkSoundEngine.SetSwitch("WalkRun", "Backwards", gameObject);}
    private void SetRunningSound() { AkSoundEngine.SetSwitch("WalkRun", "Run", gameObject);}
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
                SimpleAttackAnimation = () => { animator.SetTrigger("Attack"); animator.SetInteger("AttackAnim", UnityEngine.Random.Range(1, 4));};
                SecondaryAttackAnimation = () => { animator.SetTrigger("BigAttack");  Debug.Log("Gyaru big attack animation"); };
                GetHitAnimation = () => { animator.SetTrigger("Hurt"); animator.SetInteger("HurtAnim", UnityEngine.Random.Range(1, 4));};
                DeathAnimation = () => { animator.SetTrigger("Hurt"); animator.SetFloat("HP", -1); Debug.Log("Gyaru death animation"); };
                RunAnimation = () => { animator.SetBool("isMoving", true); animator.SetBool("isRunning", true); animator.SetBool("Backwards", false);};
                AbilityAnimation = () => Debug.Log("Gyaru ability use animation");
                InteractAnimation = () => { animator.SetTrigger("Action"); Debug.Log("Gyaru interact animation"); StartCoroutine(BlockMovementForPeriod(2f)); };
                WalkAnimation = () => { animator.SetBool("isMoving", true); animator.SetBool("isRunning", false); animator.SetBool("Backwards", false); };
                WalkBackwardsAnimation = () => { animator.SetBool("isMoving", true); animator.SetBool("isRunning", false); animator.SetBool("Backwards", true); };
                StopWalkingAnimation = () => { animator.SetBool("isMoving", false); animator.SetBool("isRunning", false); animator.SetBool("Backwards", false); };
                IdleBreakAnimation = () => { 
                    animator.SetTrigger("IdleBreak");
                    int random = UnityEngine.Random.Range(0, 2);
                    animator.SetInteger("IdleBreakAnim", random);
                    PutAndRemoveIdleBreakObject(random);
                    Debug.Log("Gyaru idle break"); 
                };
                UseAbility = () => { GameHandler.instance.Sanity += 30f * Time.deltaTime; };
                break;
            case Character.mysterious:
                SimpleAttackAnimation = () => Debug.Log("mysterious attack animation");
                SecondaryAttackAnimation = () => Debug.Log("mysterious big attack animation");
                GetHitAnimation = () => Debug.Log("mysterious get hit animation");
                DeathAnimation = () => Debug.Log("mysterious death animation");
                RunAnimation = () => Debug.Log("mysterious running animation");
                AbilityAnimation = () => Debug.Log("mysterious ability use animation");
                InteractAnimation = () => Debug.Log("mysterious interact animation");
                WalkAnimation = () =>  Debug.Log("mysterious walking animation");
                WalkBackwardsAnimation = () => Debug.Log("myserious walking backwards animation");
                StopWalkingAnimation = () => Debug.Log("mysterious stopped walking "); 
                IdleBreakAnimation = () =>  Debug.Log("mysterious idle break");
                break;
            case Character.officeworker:
                SimpleAttackAnimation = () => Debug.Log("officeworker attack animation");
                SecondaryAttackAnimation = () => Debug.Log("officeworker big attack animation");
                GetHitAnimation = () => Debug.Log("officeworker get hit animation");
                DeathAnimation = () => Debug.Log("officeworker death animation");
                RunAnimation = () => Debug.Log("officeworker running animation");
                AbilityAnimation = () => Debug.Log("officeworker ability use animation");
                InteractAnimation = () => Debug.Log("officeworker interact animation");
                WalkAnimation = () => { Debug.Log("officeworker walking animation"); };
                WalkBackwardsAnimation = () => Debug.Log("officeworker walking backwards animation");
                StopWalkingAnimation = () => Debug.Log("officeworker stopped walking "); 
                IdleBreakAnimation = () =>  Debug.Log("officeworker idle break");
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

[Serializable] public class ObjectDuration
{
    public GameObject go;
    public float duration;

    public ObjectDuration(GameObject go, float duration)
    {
        this.go = go;
        this.duration = duration;
    }
}
