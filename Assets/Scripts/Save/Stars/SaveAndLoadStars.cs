using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveAndLoadStars
{
    private static string _pathFileStars = Application.persistentDataPath + "/stars.data";

    public static bool IsExistsStarsFile() {
        if (File.Exists(_pathFileStars)) {
            return true;
        }

        return false;
    }

    public static void SaveStars(StarsData starsData) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileStars, FileMode.Create);

        formatter.Serialize(stream, starsData);
        stream.Close();
    }

    public static StarsData LoadStars() {
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_pathFileStars, FileMode.Open);

        StarsData _starsData = _formatter.Deserialize(_stream) as StarsData;
        _stream.Close();

        return _starsData;
    }
}
