using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyProfile")]
public class EnemyProfileSO : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed = 2f;

    [Tooltip("Does the enemy chase the player")]
    public bool chase;
    public float detectionRange = 10f;
    public float chaseRange = 15f;

    [Tooltip("Does the enemy run after hitting the player")]
    public bool run;
    public float runningRange;
    public float runSpeed = 4f;
    public Vector2 watchingDurationMinMax;

    [Tooltip("Does the enemy attack from distance")]
    public bool range;
    public GameObject projectilePrefab;

    [Tooltip("The range at which the enemy starts to attack")]
    public float rangeToAttack = 2f;
    [Tooltip("Max range between attackPoint and hit colliders")]
    public float attackRange = 0.5f;
    [Tooltip("Cooldown of the attack")]
    public float attackCooldown;
    [Tooltip("Is the enemy able to backstab the player")]
    public bool backstab;


    #region Wwwise events
    public AK.Wwise.Event attackWEvent;
    public AK.Wwise.Event chaseWEvent;
    public AK.Wwise.Event runWEvent;
    public float timeToPostRunEvent;
    public AK.Wwise.Event watchWEvent;

    public AK.Wwise.Event onSpawnWEvent;
    public AK.Wwise.Event getHitWEvent;
    public AK.Wwise.Event deathWEvent;

    [Tooltip("The switch that allows to the player to detect from with type of enemy the attack comes from")]
    public string hitPlayerWEventSwitch; 
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyProfileSO))]
public class EnemyProfileSOInspector : Editor
{
    private int currentTab = 0;

    public override void OnInspectorGUI()
    {
        EnemyProfileSO so = target as EnemyProfileSO;

        serializedObject.Update();

        currentTab = GUILayout.Toolbar(currentTab, new string[] { "Health", "Attack", "Chase", "Run", "Wwise Events" });

        switch (currentTab)
        {
            case 0: // Health
                #region Health stats
                CreatePropertyField(nameof(so.maxHealth));
                CreatePropertyField(nameof(so.movementSpeed));
                #endregion
                break;
            case 1: // Attack
                #region Attack stats
                CreatePropertyField(nameof(so.rangeToAttack));
                CreatePropertyField(nameof(so.attackRange));
                CreatePropertyField(nameof(so.attackCooldown));
                CreatePropertyField(nameof(so.backstab));

                GUILayout.Space(10);

                #region Range Attack stats
                CreatePropertyField(nameof(so.range));
                if (so.range) { CreatePropertyField(nameof(so.projectilePrefab));}
                #endregion
                #endregion
                break;
            case 2: // Chase
                #region Chase stats
                CreatePropertyField(nameof(so.chase));
                if (so.chase)
                {
                    CreatePropertyField(nameof(so.detectionRange));
                    CreatePropertyField(nameof(so.chaseRange));
                }
                #endregion
                break;
            case 3: // Run
                #region Run stats
                if (so.chase)
                {
                    CreatePropertyField(nameof(so.run));
                    if (so.run)
                    {
                        CreatePropertyField(nameof(so.runningRange));
                        CreatePropertyField(nameof(so.runSpeed));
                        CreatePropertyField(nameof(so.watchingDurationMinMax));
                    }
                }
                #endregion
                break;
            case 4: // Wwise Events
                #region Wwise Events
                CreatePropertyField(nameof(so.attackWEvent));
                if (!so.range) CreatePropertyField(nameof(so.hitPlayerWEventSwitch));
                GUILayout.Space(5);
                CreatePropertyField(nameof(so.onSpawnWEvent));
                if (so.maxHealth > 1) CreatePropertyField(nameof(so.getHitWEvent));
                CreatePropertyField(nameof(so.deathWEvent));
                GUILayout.Space(5);
                if (so.chase)
                {
                    CreatePropertyField(nameof(so.chaseWEvent));
                    if (so.run) { CreatePropertyField(nameof(so.runWEvent)); CreatePropertyField(nameof(so.timeToPostRunEvent)); CreatePropertyField(nameof(so.watchWEvent)); }
                }
                #endregion
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
#endif
