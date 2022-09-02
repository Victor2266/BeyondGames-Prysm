using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    public static void SavePlayerEntity (PlayerEntity player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "singleplayer1.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSaveData data = new PlayerSaveData(player);

        try
        {
            formatter.Serialize(stream, data);
            Debug.Log("Saved Player to:" + Application.persistentDataPath);
        }
        finally
        {
            stream.Close();
        }

    }
    public static void SaveEquipment()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "equipment.data");
        string path2 = Path.Combine(Application.persistentDataPath, "equipment2.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        if (EquipmentManager.instance == null)
        {
            return;
        }
        Equipment[] equipments = EquipmentManager.instance.getCurrentEquipment();
        List<string> equipmentNames = new List<string>();

        foreach (Equipment eq in equipments)
        {
            if (eq != null)
                equipmentNames.Add(eq.name);
        }
        try
        {
            formatter.Serialize(stream, equipmentNames);
            Debug.Log("Saved equipment");
        }
        finally
        {
            stream.Close();
        }

    }
    public static void SaveInventory()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "inventory.data");
        FileStream stream = new FileStream(path, FileMode.Create);
        if (Inventory.instance == null)
        {
            return;
        }
        List<Item> items = Inventory.instance.items;
        List<string> itemNames = new List<string>();
        foreach (Item eq in items)
        {
            if (eq != null)
                itemNames.Add(eq.name);
        }
        try
        {
            SaveEquipment saveEq = new SaveEquipment(itemNames);
            formatter.Serialize(stream, saveEq);
            Debug.Log("Saved inventory" + JsonUtility.ToJson(saveEq));
        }
        finally
        {
            stream.Close();
        }

    }
    public static PlayerSaveData LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath, "singleplayer1.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveData data = null;
            try
            {
                data = formatter.Deserialize(stream) as PlayerSaveData;
                Debug.Log("Loaded Player");
            }
            catch
            {
                Debug.LogError("Failed to load player data");
            }
            finally
            {
                stream.Close();
            }

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static PlayerEntity LoadPlayerEntity(PlayerEntity entity)
    {
        PlayerSaveData data = LoadPlayer();

        entity.PlayerEntityUpdate(data);

        return entity;
    }
    public static Equipment[] LoadEquipment()
    {
        string path = Path.Combine(Application.persistentDataPath, "equipment.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<string> equipmentNames = new List<string>();
            Equipment[] returnArray;
            int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            returnArray = new Equipment[numSlots];

            try
            {
                equipmentNames = formatter.Deserialize(stream) as List<string>;
                Debug.Log("Loaded equipment");
            }
            catch
            {
                Debug.LogError("Failed to load equipment data");
                stream.Close();
                return returnArray;
            }
            finally
            {
                stream.Close();

                foreach (string str in equipmentNames)
                {
                    Equipment eq = Resources.Load("Interactables/" + str) as Equipment;
                    int slotIndex = (int)eq.equipSlot;

                    returnArray[slotIndex] = eq;
                }
            }


            return returnArray;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }


    }

    internal static void deleteInventoryAndEquipment()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "inventory.data");
        FileStream stream = new FileStream(path, FileMode.Create);

        List<string> itemNames = new List<string>();
   
        try
        {
            formatter.Serialize(stream, itemNames);
            Debug.Log("deleted inventory");
        }
        finally
        {
            stream.Close();
        }

        path = Path.Combine(Application.persistentDataPath, "equipment.data");
        stream = new FileStream(path, FileMode.Create);

        try
        {
            formatter.Serialize(stream, itemNames);
            Debug.Log("deleted equipment");
        }
        finally
        {
            stream.Close();
        }
    }

    public static List<Item> LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, "inventory.data");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<string> equipmentNames = new List<string>();
            SaveEquipment SaveEq = new SaveEquipment(equipmentNames);
            List<Item> data = new List<Item>();
            try
            {
                SaveEq = formatter.Deserialize(stream) as SaveEquipment;
                Debug.Log("Loaded inventory");
            }
            catch
            {
                Debug.LogError("Failed to load inventory data");
                stream.Close();
                return data;
            }
            finally
            {
                stream.Close();

                foreach (string str in SaveEq.items)
                {
                    Item getRecourse = Resources.Load("Interactables/" + str) as Item;
                    data.Add(getRecourse);
                }
            }

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
