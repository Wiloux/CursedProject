using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDresser : MonoBehaviour
{

    public SkinnedMeshRenderer SkinnedMeshRend;
    public int CurrentClothInt = 0;
    public int CurrentHairInt = 0;
    public List<Material> AllClothes = new List<Material>();
    public List<Material> AllHair = new List<Material>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeClothing();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeHair();
        }
    }

    public void ChangeClothing()
    {
        CurrentClothInt++;
        Material currentCloth = AllClothes[CurrentClothInt % AllClothes.Count];
        Material[] mats = SkinnedMeshRend.materials;
        mats[0] = currentCloth;
        SkinnedMeshRend.materials = mats;
    }

    public void ChangeHair()
    {
        CurrentHairInt++;
        Material currentHair = AllHair[CurrentHairInt % AllHair.Count];
        Material[] mats = SkinnedMeshRend.materials;
        mats[1] = currentHair;
        SkinnedMeshRend.materials = mats;
    }
}

