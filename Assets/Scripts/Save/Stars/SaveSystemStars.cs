using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveSystemStars : MonoBehaviour
{
    public static bool IsExistsSaveStarsFile() {
        string path = Application.persistentDataPath + "/stars.data";

        if (File.Exists(path)) {
            return true;
        }

        return false;
    }

    public static void SaveStars(int countStars, int levelIndex) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stars.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        StarsData starsData = new StarsData(countStars, levelIndex);
        formatter.Serialize(stream, starsData);
        stream.Close();
    }

    public static StarsData LoadStars() {
        string path = Application.persistentDataPath + "/stars.data";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StarsData starsData = formatter.Deserialize(stream) as StarsData;
            stream.Close();

            return starsData;
        }
        else {
            Debug.LogError("Save file not found " + path);
            return null;
        }
    }
}
