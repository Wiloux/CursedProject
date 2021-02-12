using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> clues = new List<string>();
    public ObjectSO[] items;

    private void Start()
    {
       clues = new List<string>();
    }
}
