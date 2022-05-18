using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CartEnemies
{
    public List<bool> unlocksEnemies = new List<bool>();

    public CartEnemies(List<bool> unlocksEnemies) {
        this.unlocksEnemies = unlocksEnemies;
    }
}
