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
            CreatePropertyField("maxHealth");
            CreatePropertyField("movementSpeed");
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(20);

        #region Attack stats
        showAttacksStats = EditorGUILayout.BeginFoldoutHeaderGroup(showAttacksStats, "Attack stats ------------------");
        if (showAttacksStats)
        {
            CreatePropertyField("rangeToAttack");
            CreatePropertyField("attackRange");
            CreatePropertyField("attackCooldown");
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(20);

        #region Range stats
        CreatePropertyField("range");
        if (so.range)
        {
            showRangeStats = EditorGUILayout.BeginFoldoutHeaderGroup(showRangeStats, "Range stats ------------------");
            if (showRangeStats)
            {
                CreatePropertyField("projectilePrefab");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        #endregion

        GUILayout.Space(20);

        #region Chase stats
        CreatePropertyField("chase");
        if (so.chase)
        {
            showChaseStats = EditorGUILayout.BeginFoldoutHeaderGroup(showChaseStats, "Chase stats ------------------");
            if (showChaseStats)
            {
                CreatePropertyField("detectionRange");
                CreatePropertyField("chaseRange");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.Space(20);
        }
        #endregion

        #region Run stats
        if (so.chase)
        {
            CreatePropertyField("run");
            if (so.run)
            {
                showRunStats = EditorGUILayout.BeginFoldoutHeaderGroup(showRunStats, "Run stats ------------------");
                if (showRunStats)
                {
                    CreatePropertyField("runningRange");
                    CreatePropertyField("runSpeed");
                    CreatePropertyField("watchingDuration");
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
            CreatePropertyField("attackWEvent");
            if(so.maxHealth > 1) CreatePropertyField("hitWEvent");
            CreatePropertyField("deathWEvent");
            if (so.chase)
            {
                CreatePropertyField("chaseWEvent");
                if (so.run) { CreatePropertyField("runWEvent"); CreatePropertyField("watchWEvent"); }
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
