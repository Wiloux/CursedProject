using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform[] itemsIconParents = new Transform[3];
    [SerializeField] private TMP_Text nameDisplayer;
    [SerializeField] private TMP_Text descriptionDisplayer;

    private int currentItemIndex;
    private ObjectSO[] items;
    private List<GameObject> previewItems = new List<GameObject>() { null, null, null};

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

        // Rotate the preview items
        for(int i = 0; i < 3;i++)
        {
            int index = currentItemIndex - 1 + i;
            if (currentItemIndex == 0) index += 1;
            if(previewItems[i] != null) previewItems[i].transform.Rotate(items[index].previewRotation * Time.deltaTime);
        }
    }

    private void UpdateDisplay()
    {
        // Destroy all preview Items
        foreach (Transform itemIconParent in itemsIconParents)
        {
            for(int i =0; i < itemIconParent.childCount;i++) { Destroy(itemIconParent.GetChild(0).gameObject); }
        }
        // Destroy the previewItems list's elements
        for(int i = 0; i < 3; i++)
        {
            previewItems[i] = null;
        }


        // Check which case need a preview, if doesn't we disabled them
        for (int i = 0; i < 3; i++)
        {
            int index = currentItemIndex - 1 + i;
            bool show = false;
            if(0 <= index && index < items.Length)
            {
                CreatePreviewInCase(items[index], i);
                show = true;
            }
            itemsIconParents[i].gameObject.SetActive(show);

            // We change the text ui with the object profile displayed on the central case
            if(i == 1)
            {
                nameDisplayer.text = items[index].objectName;
                descriptionDisplayer.text = items[index].longDescription;
            }
        }
    }

    private void CreatePreviewInCase(ObjectSO currentItem, int index)
    {
        // Create the "preview object"
        GameObject preview = Instantiate(currentItem.objectModel, Vector3.zero, Quaternion.identity);

        // Place it 
        preview.transform.SetParent(itemsIconParents[index].transform);
        preview.transform.localPosition = Vector3.zero;
        // Scale it
        float scale = currentItem.scaleValue;
        preview.transform.localScale = new Vector3(scale, scale, scale);
        // Rotate it
        preview.transform.localRotation = Quaternion.Euler(currentItem.previewStartRotation);

        // Add it to the preview Items list to rotate them later
        previewItems[index] = preview;
    }
}
