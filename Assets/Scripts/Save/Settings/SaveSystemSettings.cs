using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveSystemSettings
{
    public static bool IsExistsSaveSettingsFile() {
        string path = Application.persistentDataPath + "/settings.data";

        if (File.Exists(path)) {
            return true;
        }

        return false;
    }

    public static void SaveSettings(SettingsMenu settingsMenu) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData settingsData = new SettingsData(settingsMenu);
        //Debug.Log("save index = " + settingsData.indexResolution);
        formatter.Serialize(stream, settingsData);
        stream.Close();
    }

    public static SettingsData LoadSettings() {
        string path = Application.persistentDataPath + "/settings.data";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData SettingsData = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return SettingsData;
        }
        else {
            Debug.LogError("Save file not found " + path);
            return null;
        }
    }
}
