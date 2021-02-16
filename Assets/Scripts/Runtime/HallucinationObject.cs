using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HallucinationObject : MonoBehaviour
{
    [SerializeField] private bool interactible;

    private enum InteractibleType
    {
        Door,
        Dialogue,
        IDK,
    }
    [SerializeField] private InteractibleType interactibleType;

    private Action EnableInteractivity;
    private Action DisableInteractivity;
    private MonoBehaviour compo;

    // Start is called before the first frame update
    void Start()
    {
        if (interactible)
        {
            switch (interactibleType)
            {
                case InteractibleType.Dialogue:
                    break;
                case InteractibleType.Door:
                    compo = GetComponent<DoorScript>();
                    break;
                case InteractibleType.IDK:
                    //compo = GetComponent<EnemyBaseAI>();
                    EnableInteractivity = () => GetComponent<EnemyBaseAI>().enabled = true;
                    DisableInteractivity = () => GetComponent<EnemyBaseAI>().enabled = false;
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
}

#if UNITY_EDITOR
[CustomEditor(typeof(HallucinationObject))]
public class HallucinationObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


    }
}
#endif