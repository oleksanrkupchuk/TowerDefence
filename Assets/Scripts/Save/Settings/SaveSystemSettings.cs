using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveSystemSettings {
    private static string _pathFileSettings = Application.persistentDataPath + "/settings.data";

    public static bool IsExistsSaveSettingsFile() {
        if (File.Exists(_pathFileSettings)) {
            return true;
        }

        return false;
    }

    public static void SaveSettings(SettingsData settingsData) {
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_pathFileSettings, FileMode.Create);

        _formatter.Serialize(_stream, settingsData);
        _stream.Close();
    }

    public static SettingsData LoadSettings() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileSettings, FileMode.Open);

        SettingsData SettingsData = formatter.Deserialize(stream) as SettingsData;
        stream.Close();

        return SettingsData;
    }
}
