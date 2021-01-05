using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStatsSO : ScriptableObject
{
    public int maxHealth;

    [Tooltip("Does the enemy chase the player")]
    public bool chase;
    public float detectionRange = 10f;
    public float chaseRange = 15f;

    [Tooltip("Does the enemy run after hitting the player")]
    public bool run;
    public float runningRange;
    public float runSpeed = 4f;
    public float watchingDuration;

    [Tooltip("Does the enemy attack from distance")]
    public bool range;
    public GameObject projectilePrefab;

    [Tooltip("The range at which the enemy starts to attack")]
    public float rangeToAttack = 2f;
    [Tooltip("Max range between attackPoint and hit colliders")]
    public float attackRange = 0.5f;

    [Tooltip("Cooldown of the attack")]
    public float attackCooldown;

    public float movementSpeed = 2f;
}
