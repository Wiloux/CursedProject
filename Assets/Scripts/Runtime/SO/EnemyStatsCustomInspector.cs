using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyStatsSO))]
public class EnemyStatsCustomInspector : Editor
{
    private bool showHealthStats;
    private bool showChaseStats;
    private bool showRunStats;
    private bool showRangeStats;
    private bool showAttacksStats;

    public override void OnInspectorGUI()
    {
        EnemyStatsSO so = target as EnemyStatsSO;

        serializedObject.Update();

        #region Health stats
        GUILayout.Label("Health ------------------");
        showHealthStats = EditorGUILayout.BeginFoldoutHeaderGroup(showHealthStats, "Health stats");
        if (showHealthStats)
        {
            CreatePropertyField("maxHealth");
            CreatePropertyField("movementSpeed");
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(10);

        #region Attack stats
        GUILayout.Label("Attack ------------------");
        showAttacksStats = EditorGUILayout.BeginFoldoutHeaderGroup(showAttacksStats, "Attack stats");
        if (showAttacksStats)
        {
            CreatePropertyField("rangeToAttack");
            CreatePropertyField("attackRange");
            CreatePropertyField("attackCooldown");
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(10);

        #region Range stats
        GUILayout.Label("Range ------------------");
        CreatePropertyField("range");
        if (so.range)
        {
            showRangeStats = EditorGUILayout.BeginFoldoutHeaderGroup(showRangeStats, "Range stats");
            if (showRangeStats)
            {
                CreatePropertyField("projectilePrefab");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        #endregion

        GUILayout.Space(10);

        #region Chase stats
        GUILayout.Label("Chase ------------------");
        GUILayout.Space(5);
        CreatePropertyField("chase");
        if (so.chase)
        {
            showChaseStats = EditorGUILayout.BeginFoldoutHeaderGroup(showChaseStats, "Chase stats");
            if (showChaseStats)
            {
                CreatePropertyField("detectionRange");
                CreatePropertyField("chaseRange");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.Space(10);
        }
        #endregion

        #region Run stats
        if (so.chase)
        {
            GUILayout.Label("Run ------------------");
            GUILayout.Space(5);
            showRunStats = EditorGUILayout.BeginFoldoutHeaderGroup(showRunStats, "Run stats");
            if (showRunStats)
            {
                CreatePropertyField("run");
                if (so.run)
                {
                    CreatePropertyField("runningRange");
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
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
