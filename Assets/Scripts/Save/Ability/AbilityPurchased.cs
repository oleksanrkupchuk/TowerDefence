using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityPurchased {

    public List<AbilityItem> abilities = new List<AbilityItem>();

    public AbilityPurchased(List<AbilityItem> items) {
        //AbilityItem _item = new AbilityItem(type, isPurchased);
        Debug.Log("before save item, list count = " + abilities.Count);
        abilities = items;
        Debug.Log("after save item, list count = " + abilities.Count);
    }

    public AbilityPurchased(AbilityItem item) {
        //AbilityItem _item = new AbilityItem(type, isPurchased);
        Debug.Log("before save item, list count = " + abilities.Count);
        abilities.Add(item);
        Debug.Log("after save item, list count = " + abilities.Count);
    }
}

[Serializable]
public class AbilityItem {
    public AbilityType type = new AbilityType();
    public bool isPurchased;

    public AbilityItem(AbilityType type, bool isPurchased) {
        this.type = type;
        this.isPurchased = isPurchased;
    }
}