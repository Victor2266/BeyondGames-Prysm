using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

}
