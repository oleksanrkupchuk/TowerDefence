[System.Serializable]
public class AbilityPurchase 
{
    public bool[] isPurchased = new bool[7];

    public AbilityPurchase(int number, bool isPurchase) {
        isPurchased[number] = isPurchase;
    }
}
