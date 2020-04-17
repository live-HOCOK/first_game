using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
    private static string filePath = Application.persistentDataPath + "/save.kat";

    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);


        bf.Serialize(fs, GameState.SaveState());

        fs.Close();
    }

    public static bool LoadGame()
    {
        if (!CheckSave())
            return false;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        GameState.LoadState((GameState.AllState)bf.Deserialize(fs));

        return true;
    }

    public static bool CheckSave()
    {
        return File.Exists(filePath);
    }
}
