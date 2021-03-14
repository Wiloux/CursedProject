using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public Player_Movement controller;
    public Player player;
    public Inventory inventory;
    public static PlayerHelper instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }


    #region Inventory
        #region clues
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
    #endregion

        #region inv
    public List<ObjectSO> GetPlayerInventory()
    {
        return inventory.items;
    }
    public List<KeySO> GetPlayerKeys() { return inventory.GetInventoryKeys(); }
            #region Add/Remove Inventory
    public void AddObjectToPlayerInventory(ObjectSO objectToAdd) { inventory.AddObjectToInv(objectToAdd); }
    public void RemoveObjectFromPlayerInvenotry(ObjectSO objectToRemove) { inventory.RemoveObjectFromInv(objectToRemove); }
    #endregion 

            #region Healing Items gestion

    public int GetNumberOfHealingItems() { return inventory.healingItemInInv; }
    public void AdjustNumberOfHealingItem(int adjustment) { inventory.healingItemInInv += adjustment; if (inventory.healingItemInInv > inventory.healingItemCapacity) FillUpHealInBag(); }
    public void FillUpHealInBag() { inventory.healingItemInInv = inventory.healingItemCapacity; }
    #endregion
    #endregion
    #endregion

    #region Player
    public void ToggleControls() {
        player.stopControlls = !player.stopControlls;
    }
    public void SetMouseSensivity(float newSensivity) { controller.sensivity = newSensivity * 20f; }
    public void SetKeyBindings(KeyBindings keyBindings) { controller.keyBindings = keyBindings; player.keyBindings = keyBindings; }

    public float GetPlayerLife() { return player.health; }

    public void TakeDamage(float damage, bool stagger = true)
    {
        player.TakeDamage(damage, stagger);
    }

    public void Die()
    {
        player.Die();
    }
    #endregion

    #region UI

    #endregion
}
