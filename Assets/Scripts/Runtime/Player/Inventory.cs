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
}
