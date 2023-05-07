using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;
    public GameObject inventoryPanel; //needs to be active and then turned off after equipment is populated
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            Destroy(this);
            return;
        }
        instance = this;

    }

    #endregion

    Equipment[] currentEquipment; // actually wearing these items

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;

        currentEquipment = SaveSystem.LoadEquipment();
        if (currentEquipment == null)
            currentEquipment = new Equipment[numSlots];

        StartCoroutine(waitAndRefresh());
    }


    public void Equip (Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = null;
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            currentEquipment[slotIndex] = newItem;
            inventory.Add(oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        TooltipManager.Hide();
    }


    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            Debug.Log("UNEQUIPPING " + oldItem.name);
            currentEquipment[slotIndex] = null;
            inventory.Add(oldItem);

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
        
    }
    public bool isEquipped(int slotIndex)
    {
        return !(currentEquipment[slotIndex +1] == null);
    }
    public GameObject[] getSpells(int spellNumber)
    {
        GameObject[] ret = new GameObject[2];
        Spell basicVer = (Spell)currentEquipment[spellNumber +1];
        ret[0] = basicVer.SpellPrefab;
        Spell bigVer = (Spell)currentEquipment[spellNumber +1];
        ret[1] = bigVer.ChargedSpellPrefab;
        return ret;
    }

    public Color getColor(int index)
    {
        return currentEquipment[index + 1].color;
    }

    public InventoryUI.WeaponTypes getWeaponType(int index)
    {
        if (currentEquipment[index + 1] != null)
            return currentEquipment[index + 1].WeaponType;
        else
            return InventoryUI.WeaponTypes.All;
    }

    public Weapon getMeleeWeapon()
    {
        return (Weapon)currentEquipment[0];
    }
    public void resetInventory()
    {
        currentEquipment = new Equipment[System.Enum.GetNames(typeof(EquipmentSlot)).Length];
        Inventory.instance.items = new List<Item>();
    }
    public Equipment[] getCurrentEquipment()
    {
        return currentEquipment;
    }
    public bool Contains(Item item)
    {
        foreach (Equipment eq in currentEquipment)
        {
            if(eq != null)
            {

                if (eq.name == item.name)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void setCurrentEquipment(Equipment[] loadedEquipment)
    {
        currentEquipment = loadedEquipment;
    }

    IEnumerator waitAndRefresh()
    {
        yield return new WaitForSeconds(0.01f);
        foreach (Equipment newItem in currentEquipment)
        {
            if (newItem != null)
            {
                if (onEquipmentChanged != null)
                {
                    onEquipmentChanged.Invoke(newItem, null);
                }
            }
        }
        inventoryPanel.SetActive(false);
    } 
}
