using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyProfileSO))]
public class EnemyProfileCustomInspector : Editor
{
    private bool showHealthStats;
    private bool showChaseStats;
    private bool showRunStats;
    private bool showRangeStats;
    private bool showAttacksStats;
    private bool showWwiseEvents;
    public override void OnInspectorGUI()
    {
        EnemyProfileSO so = target as EnemyProfileSO;

        serializedObject.Update();

        #region Health stats
        showHealthStats = EditorGUILayout.BeginFoldoutHeaderGroup(showHealthStats, "Health stats ------------------");
        if (showHealthStats)
        {
            CreatePropertyField(nameof(so.maxHealth));
            CreatePropertyField(nameof(so.movementSpeed));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(20);

        #region Attack stats
        showAttacksStats = EditorGUILayout.BeginFoldoutHeaderGroup(showAttacksStats, "Attack stats ------------------");
        if (showAttacksStats)
        {
            CreatePropertyField(nameof(so.rangeToAttack));
            CreatePropertyField(nameof(so.attackRange));
            CreatePropertyField(nameof(so.attackCooldown));
            CreatePropertyField(nameof(so.backstab));
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(20);

        #region Range stats
        CreatePropertyField(nameof(so.range));
        if (so.range)
        {
            showRangeStats = EditorGUILayout.BeginFoldoutHeaderGroup(showRangeStats, "Range stats ------------------");
            if (showRangeStats)
            {
                CreatePropertyField(nameof(so.projectilePrefab));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        #endregion

        GUILayout.Space(20);

        #region Chase stats
        CreatePropertyField(nameof(so.chase));
        if (so.chase)
        {
            showChaseStats = EditorGUILayout.BeginFoldoutHeaderGroup(showChaseStats, "Chase stats ------------------");
            if (showChaseStats)
            {
                CreatePropertyField(nameof(so.detectionRange));
                CreatePropertyField(nameof(so.chaseRange));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.Space(20);
        }
        #endregion

        #region Run stats
        if (so.chase)
        {
            CreatePropertyField(nameof(so.run));
            if (so.run)
            {
                showRunStats = EditorGUILayout.BeginFoldoutHeaderGroup(showRunStats, "Run stats ------------------");
                if (showRunStats)
                {
                    CreatePropertyField(nameof(so.runningRange));
                    CreatePropertyField(nameof(so.runSpeed));
                    CreatePropertyField(nameof(so.watchingDurationMinMax));
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        #endregion

        GUILayout.Space(30);

        #region Wwise Events
        showWwiseEvents = EditorGUILayout.BeginFoldoutHeaderGroup(showWwiseEvents, "Wwise Events");
        if (showWwiseEvents)
        {
            CreatePropertyField(nameof(so.attackWEvent));
            if(so.maxHealth > 1) CreatePropertyField(nameof(so.hitWEvent));
            CreatePropertyField(nameof(so.deathWEvent));
            if (so.chase)
            {
                CreatePropertyField(nameof(so.chaseWEvent));
                if (so.run) { CreatePropertyField(nameof(so.runWEvent)); CreatePropertyField(nameof(so.timeToPostRunEvent)); CreatePropertyField(nameof(so.watchWEvent)); }
            }
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
