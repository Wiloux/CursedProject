using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private MeshFilter[] itemsRenderers = new MeshFilter[3];
    [SerializeField] private Image[] itemsImages = new Image[3];
    [SerializeField] private TMP_Text nameDisplayer;
    [SerializeField] private TMP_Text descriptionDisplayer;

    private int currentItemIndex;
    private ObjectSO[] items;

    private void OnEnable()
    {
        items = PlayerHelper.instance.GetPlayerInventory();
        currentItemIndex = 0;
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) { if (currentItemIndex > 0) currentItemIndex--; UpdateDisplay(); }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) { if (currentItemIndex < items.Length - 1) currentItemIndex++; UpdateDisplay(); }


    }

    private void UpdateDisplay()
    {
        ObjectSO currentItem = items[currentItemIndex];
        //itemsRenderers[1].sharedMesh = currentItem.objectModel.GetComponent<MeshFilter>().sharedMesh;
        //itemsRenderers[1].GetComponent<MeshRenderer>().sharedMaterials = currentItem.objectModel.GetComponent<MeshRenderer>().sharedMaterials;
        SetObjectRenderToCase(currentItem, 1);
        //itemsImages[1].sprite = currentItem.objectSprite;
        nameDisplayer.text = currentItem.objectName;
        descriptionDisplayer.text = currentItem.longDescription;
        

        int index = currentItemIndex - 1; 
        for(int i = 0; i < 3; i+= 2)
        {
            bool temp = false;
            if(0 <= index && index < items.Length)
            {
                ////itemsImages[i].sprite = items[index].objectSprite;
                //itemsRenderers[i].sharedMesh = currentItem.objectModel.GetComponent<MeshFilter>().sharedMesh;
                //itemsRenderers[i].GetComponent<MeshRenderer>().sharedMaterials = currentItem.objectModel.GetComponent<MeshRenderer>().sharedMaterials;
                SetObjectRenderToCase(items[index], i);
                temp = true;
            }
            itemsImages[i].gameObject.SetActive(temp);
            index = currentItemIndex + 1;
        }
    }

    private void SetObjectRenderToCase(ObjectSO currentItem, int index)
    {
        // Mesh part
        MeshFilter meshFilter;
        Mesh sharedMesh = null;

        if(currentItem.objectModel.TryGetComponent<MeshFilter>(out meshFilter)) sharedMesh = meshFilter.sharedMesh;
        else sharedMesh = currentItem.objectModel.GetComponentInChildren<MeshFilter>().sharedMesh;

        if (sharedMesh != null) itemsRenderers[index].sharedMesh = sharedMesh;
        else Debug.LogWarning("SharedMesh of the " + currentItem.objectName + " object profile are not found");

        // Material(s) part

        MeshRenderer meshRenderer;
        Material[] sharedMaterials;

        if (currentItem.objectModel.TryGetComponent<MeshRenderer>(out meshRenderer)) sharedMaterials = meshRenderer.sharedMaterials;
        else sharedMaterials = currentItem.objectModel.GetComponentInChildren<MeshRenderer>().sharedMaterials;

        if (sharedMaterials != null) itemsRenderers[index].GetComponent<MeshRenderer>().sharedMaterials = sharedMaterials;
        else Debug.LogWarning("SharedMaterials of the " + currentItem.objectName + " object profile are not found");
    }
}
