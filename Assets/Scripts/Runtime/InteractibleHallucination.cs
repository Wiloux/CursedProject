using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractibleHallucination : MonoBehaviour
{
    public float sanityLevelRequired;
    public bool interactible;
    public enum InteractibleType
    {
        Door,
        Dialogue,
        EnemyBaseAiTEST,
    }
    public InteractibleType interactibleType;

    private Action EnableInteractivity;
    private Action DisableInteractivity;
    

    // Start is called before the first frame update
    void Start()
    {
        if (interactible)
        {
            
            switch (interactibleType)
            {
                case InteractibleType.Dialogue:
                    SetToggleInteractivityActions<DialogInteraction>();
                    break;
                case InteractibleType.Door:
                    SetToggleInteractivityActions<DoorScript>();
                    break;
                case InteractibleType.EnemyBaseAiTEST:
                    SetToggleInteractivityActions<EnemyBaseAI>();
                    break;
            }
            DisableInteractivity();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactible)
        {
            if(GameHandler.instance.Sanity >= 100)
            {
                EnableInteractivity();
            }
            else { DisableInteractivity(); }
        }
    }

    private void SetToggleInteractivityActions<T>() where T : MonoBehaviour
    {
        EnableInteractivity = () => GetComponent<T>().enabled = true;
        DisableInteractivity = () => GetComponent<T>().enabled = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InteractibleHallucination))]
public class HallucinationObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        InteractibleHallucination hallu = target as InteractibleHallucination;

        serializedObject.Update();

        CreatePropertyField(nameof(hallu.sanityLevelRequired));

        GUILayout.Space(10);

        CreatePropertyField(nameof(hallu.interactible));
        if (hallu.interactible)
        {
            CreatePropertyField(nameof(hallu.interactibleType));
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