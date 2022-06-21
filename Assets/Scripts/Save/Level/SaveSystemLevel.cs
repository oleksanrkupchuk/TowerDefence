using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystemLevel : MonoBehaviour {
    private static string _pathFileLevel = Application.persistentDataPath + "/levels.data";

    public static bool IsExistsLevelsFile() {
        if (File.Exists(_pathFileLevel)) {
            return true;
        }

        return false;
    }

    public static void SaveLevels(List<Level> levels) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileLevel, FileMode.Create);

        formatter.Serialize(stream, levels);
        stream.Close();
    }

    public static List<Level> LoadLevels() {
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_pathFileLevel, FileMode.Open);

        List<Level> _levels = _formatter.Deserialize(_stream) as List<Level>;
        _stream.Close();

        return _levels;
    }
}
