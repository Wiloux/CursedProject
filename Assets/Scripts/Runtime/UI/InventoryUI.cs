using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
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
        itemsImages[1].sprite = currentItem.objectSprite;
        nameDisplayer.text = currentItem.objectName;
        descriptionDisplayer.text = currentItem.longDescription;
        

        int index = currentItemIndex - 1; 
        for(int i = 0; i < 3; i+= 2)
        {
            bool temp = false;
            if(0 <= index && index < items.Length)
            {
                itemsImages[i].sprite = items[index].objectSprite;
                temp = true;
            }
            itemsImages[i].gameObject.SetActive(temp);
            index = currentItemIndex + 1;
        }
    }
}
