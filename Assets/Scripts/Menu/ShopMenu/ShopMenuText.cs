using UnityEngine;

public class ShopMenuText 
{
    public static readonly string titleScreen = "Shop";
    public static readonly string notEnoughMoneyDescription = "You have not enough money on the ability";

    public static string GetDescriptionForModalWindow(int value) {
        return $"You really want to buy this ability for {value} stars";
    }
}
