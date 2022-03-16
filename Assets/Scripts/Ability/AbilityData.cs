using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData", order = 1)]
public class AbilityData: ScriptableObject
{
    public AbilityType type = new AbilityType();
    public Sprite icon;
    public Sprite iconAfterPurchased;
    public int price;
    //public int index;
    public string description;
    public bool isPurchased;
}
