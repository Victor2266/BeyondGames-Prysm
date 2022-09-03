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
        string path = Path.Combine(Application.persistentDataPath, "equipment.json");

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
            SaveEquipment saveEq = new SaveEquipment(equipmentNames);
            File.WriteAllText(path, JsonUtility.ToJson(saveEq));
            Debug.Log("Saved equipment");
        }
        catch
        {
            Debug.LogError("can't save equipment");
        }
        finally
        {
        }

    }
    public static void SaveInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, "inventory.json");
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
            //formatter.Serialize(stream, saveEq);
            File.WriteAllText(path, JsonUtility.ToJson(saveEq));
            Debug.Log("Saved inventory");
        }
        catch
        {
            Debug.LogError("can't save equipment");
        }
        finally
        {
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

        if (data != null)
        {
            entity.PlayerEntityUpdate(data);

            return entity;
        }
        else
        {
            return null;
        }
    }
    public static Equipment[] LoadEquipment()
    {
        string path = Path.Combine(Application.persistentDataPath, "equipment.json");
        if (File.Exists(path))
        {
            string equipmentNames = File.ReadAllText(path);
            Equipment[] returnArray;
            int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            returnArray = new Equipment[numSlots];

            SaveEquipment SaveEq = new SaveEquipment(new List<string>());

            try
            {
                SaveEq = JsonUtility.FromJson<SaveEquipment>(equipmentNames);
                Debug.Log("Loaded equipment");
            }
            catch
            {
                Debug.LogWarning("Failed to load equipment data");
                return returnArray;
            }
            finally
            {
                foreach (string str in SaveEq.items)
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
    public static List<Item> LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, "inventory.json");
        if (File.Exists(path))
        {

            string equipmentNames = File.ReadAllText(path);
            SaveEquipment SaveEq = new SaveEquipment(new List<string>());

            List<Item> data = new List<Item>();
            try
            {
                SaveEq = JsonUtility.FromJson<SaveEquipment>(equipmentNames);
                Debug.Log("Loaded inventory");
            }
            catch
            {
                Debug.LogWarning("Failed to load inventory data");
                return data;
            }
            finally
            {
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

    internal static void deleteInventoryAndEquipment()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "inventory.json");
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

        path = Path.Combine(Application.persistentDataPath, "equipment.json");
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


}
