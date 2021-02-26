using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> clues = new List<string>();
    public List<ObjectSO> items = new List<ObjectSO>();

    private void Start()
    {
       clues = new List<string>();
    }

    public void AddObjectToInv(ObjectSO newObject) { items.Add(newObject); }
    public List<KeySO> GetInventoryKeys()
    {
        List<KeySO> keys = new List<KeySO>();
        foreach(ObjectSO item in items)
        {
            if(item.type == ObjectSO.TypeOfObject.key)
            {
                keys.Add(item.key);
            }
        }
        return keys;
    }
    public void RemoveObjectFromInv(ObjectSO objectToRemove)
    {
        if (items.Contains(objectToRemove)) items.Remove(objectToRemove);
        else Debug.LogError("Custom error: Object to remove is not found in the player invenotry");
    }
}
