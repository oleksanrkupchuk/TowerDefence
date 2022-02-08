using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class AbilitySaveSystem : MonoBehaviour {
    public static bool IsExistsSaveAbilityFile() {
        string path = Application.persistentDataPath + "/ability.data";

        if (File.Exists(path)) {
            return true;
        }

        return false;
    }

    public static void SaveAbility(List<AbilityItem> items) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ability.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        AbilityPurchased abilityPurchase = new AbilityPurchased(items);
        formatter.Serialize(stream, abilityPurchase);
        stream.Close();
    }

    public static void SaveAbility(AbilityItem item) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ability.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        AbilityPurchased abilityPurchase = new AbilityPurchased(item);
        formatter.Serialize(stream, abilityPurchase);
        stream.Close();
    }

    public static AbilityPurchased LoadAbility() {
        string path = Application.persistentDataPath + "/ability.data";
        //print(path);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        AbilityPurchased abilityPurchase = formatter.Deserialize(stream) as AbilityPurchased;
        stream.Close();

        return abilityPurchase;
    }
}
