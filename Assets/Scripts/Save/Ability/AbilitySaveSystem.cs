using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class AbilitySaveSystem : MonoBehaviour {
    public static bool IsExistsSaveAbilityFile() {
        string path = Application.persistentDataPath + "/ability.data";

        if (File.Exists(path)) {
            return true;
        }

        return false;
    }

    public static void SaveAbility(int number, bool isPurchase) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ability.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        AbilityPurchase abilityPurchase = new AbilityPurchase(number, isPurchase);
        formatter.Serialize(stream, abilityPurchase);
        stream.Close();
    }

    public static AbilityPurchase LoadAbility() {
        string path = Application.persistentDataPath + "/ability.data";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        AbilityPurchase abilityPurchase = formatter.Deserialize(stream) as AbilityPurchase;
        stream.Close();

        return abilityPurchase;
    }
}
