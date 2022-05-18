using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityPurchased {

    public List<AbilityItem> abilities = new List<AbilityItem>();

    public AbilityPurchased(List<AbilityItem> items) {
        abilities = items;
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