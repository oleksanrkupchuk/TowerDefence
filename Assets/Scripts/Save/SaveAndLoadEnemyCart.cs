using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveAndLoadEnemyCart {
    private static string _pathFileEnemyCart = Application.persistentDataPath + "/enemyCart.data";

    public static bool IsExistsSaveEnemyCartFile() {
        if (File.Exists(_pathFileEnemyCart)) {
            return true;
        }

        return false;
    }

    public static void SaveEnemyCart(List<bool> isUnlockEnemies) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileEnemyCart, FileMode.OpenOrCreate);

        CartEnemies cartEnemy = new CartEnemies(isUnlockEnemies);
        formatter.Serialize(stream, cartEnemy);
        stream.Close();
    }

    public static CartEnemies LoadEnemyCart() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_pathFileEnemyCart, FileMode.Open);

        CartEnemies cartEnemy = formatter.Deserialize(stream) as CartEnemies;
        stream.Close();

        return cartEnemy;
    }
}