using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystemLevel : MonoBehaviour {
    private static string _pathFileLevel = Application.persistentDataPath + "/levels.data";

    public static bool IsExistsSaveLevelsFile() {
        if (File.Exists(_pathFileLevel)) {
            return true;
        }

        return false;
    }

    public static void SaveLevel(int countStars, List<Level> levels) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileLevel, FileMode.Create);

        LevelData levelData = new LevelData(countStars, levels);
        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    public static LevelData LoadLevelData() {
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_pathFileLevel, FileMode.Open);

        LevelData _levelData = _formatter.Deserialize(_stream) as LevelData;
        _stream.Close();

        return _levelData;
    }
}
