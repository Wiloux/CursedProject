using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public Player_Movement playerMovement;
    public Player player;
    public Inventory inventory;
    public static PlayerHelper instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }


    #region Inventory
    public void AddClueToInventory(string clue)
    {
        inventory.clues.Add(clue);
    }

    public void RemoveAllClues()
    {
        inventory.clues.Clear();
    }

    public bool CheckIfClue(string clue)
    {
        foreach(string _clue in inventory.clues)
        {
            if(_clue == clue)
            {
                return true;
            }
        }
        return false;
    }
    public ObjectSO[] GetPlayerInventory()
    {
        return inventory.items;
    }
    #endregion

    #region Player
    public void ToggleControls() {
        player.stopControlls = !player.stopControlls;
    }

    public void TakeDamage(float damage, bool stagger = true)
    {
        player.TakeDamage(damage, stagger);
    }
    #endregion
}
