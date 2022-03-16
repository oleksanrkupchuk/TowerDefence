using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveSystemSettings {
    private static string _pathFileSound = Application.persistentDataPath + "/sound.data";
    private static string _pathFileSettings = Application.persistentDataPath + "/settings.data";

    #region SCREEN RESOLUTION
    public static bool IsExistsSaveSettingsFile() {
        if (File.Exists(_pathFileSettings)) {
            return true;
        }

        return false;
    }

    public static void SaveSettings(SettingsMenu settingsMenu) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileSettings, FileMode.Create);

        SettingsData settingsData = new SettingsData(settingsMenu);
        formatter.Serialize(stream, settingsData);
        stream.Close();
    }

    public static SettingsData LoadSettings() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileSettings, FileMode.Open);

        SettingsData SettingsData = formatter.Deserialize(stream) as SettingsData;
        stream.Close();

        return SettingsData;
    }
    #endregion

    #region SOUND
    public static bool IsExistsSaveSoundFile() {
        if (File.Exists(_pathFileSound)) {
            return true;
        }

        return false;
    }

    public static void SaveSoundData(float volume) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_pathFileSound, FileMode.Create);

        SaveSoundData _soundData = new SaveSoundData(volume);
        formatter.Serialize(_stream, _soundData);
        _stream.Close();
    }

    public static SaveSoundData LoadSound() {
        BinaryFormatter _formatter = new BinaryFormatter();
        FileStream _stream = new FileStream(_pathFileSound, FileMode.Open);

        SaveSoundData _soundData = _formatter.Deserialize(_stream) as SaveSoundData;
        _stream.Close();

        return _soundData;
    }
    #endregion
}
