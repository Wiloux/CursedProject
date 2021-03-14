using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerCustomInspector : Editor
{
    private int mainTab;
    private int generalTab;

    private string text;
    public override void OnInspectorGUI()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 20;

        serializedObject.Update();

        GUILayout.Space(15);

        Player player = target as Player;

        mainTab = GUILayout.Toolbar(mainTab, new string[] { "General", "Wwise", "VFX" });

        switch (mainTab)
        {
            case 0:
                GUILayout.Space(5);
                generalTab = GUILayout.Toolbar(generalTab, new string[] { "General", "Attack/Ability", "Idle Breaks" });
                switch (generalTab)
                {
                    case 0:
                        GUILayout.Label("General", titleStyle);
                        GUILayout.Space(10);
                        CreatePropertyField(nameof(player.controller));
                        CreatePropertyField(nameof(player.animator));
                        GUILayout.Space(5);
                        CreatePropertyField(nameof(player.character));
                        GUILayout.Space(5);
                        CreatePropertyField(nameof(player.healthMax));
                        CreatePropertyField(nameof(player.health));
                        GUILayout.Space(5);
                        CreatePropertyField(nameof(player.stopControlls));
                        break;
                    case 1:
                        GUILayout.Label("Attack", titleStyle);
                        GUILayout.Space(10);
                        CreatePropertyField(nameof(player.attackWeapon));
                        CreatePropertyField(nameof(player.attackPointRange));
                        CreatePropertyField(nameof(player.attackLayerMask));
                        GUILayout.Space(5);
                        CreatePropertyField(nameof(player.attackCooldown));
                        CreatePropertyField(nameof(player.secondaryAttackCooldown));
                        GUILayout.Space(5);
                        CreatePropertyField(nameof(player.beingArmedDuration));
                        GUILayout.Space(10);
                        GUILayout.Label("Ability", titleStyle);
                        GUILayout.Space(10);
                        CreatePropertyField(nameof(player.abilityCooldown));
                        break;
                    case 2:
                        GUILayout.Label("Idle Breaks", titleStyle);
                        GUILayout.Space(10);
                        CreatePropertyField(nameof(player.idleBreakTimerMinMax));
                        CreatePropertyField(nameof(player.idleBreaksObjects));
                        break;
                }
                break;
            case 1:
                GUILayout.Label("Wwise Events", titleStyle);
                GUILayout.Space(10);
                CreatePropertyField(nameof(player.playerAttackWEvent));
                CreatePropertyField(nameof(player.PlayerHitEvent));
                GUILayout.Space(5);
                CreatePropertyField(nameof(player.WalkRunWSwitch));
                GUILayout.Space(5);
                CreatePropertyField(nameof(player.simpleAttackWEvent));
                CreatePropertyField(nameof(player.secondaryAttackWEvent));
                break;
            case 2:
                GUILayout.Label("VFX", titleStyle);
                GUILayout.Space(10);
                CreatePropertyField(nameof(player.attackWeaponParticles));
                CreatePropertyField(nameof(player.weaponImpactParticles));
                CreatePropertyField(nameof(player.bloodParticlesPrefab));
                break;
        }

        GUILayout.Space(30);

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextField(text);
        if(GUILayout.Button("Play playerHitEvent"))
        {
            player.OnHit(text);
        }
        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
