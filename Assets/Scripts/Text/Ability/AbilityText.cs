using System.Collections.Generic;

public class AbilityText 
{
    private static Dictionary<AbilityType, string> descriptions = new Dictionary<AbilityType, string>();

    public static void Init() {
        descriptions.Add(AbilityType.SpeedShoot, speedShoot);
        descriptions.Add(AbilityType.Thorn, thorn);
        descriptions.Add(AbilityType.Burning, burning);
        descriptions.Add(AbilityType.FireArea, fireArea);
        descriptions.Add(AbilityType.Explosion, explosion);
        descriptions.Add(AbilityType.ReducePriceTower, reducePriceTower);
        descriptions.Add(AbilityType.IncreasePriceSell, increasePriceSell);
    }

    public static string GetDescription(AbilityType type) {
        return descriptions[type];
    }

    private static readonly string fireArea = "When bullet of fire tower touch earth is being created circle of fire. Enemies which cross circle of fire burning some time";
    private static readonly string explosion = "When bullet of rock tower touch earth happens very strong explosion, which give damage surrounding enemies";
    private static readonly string burning = "";
    private static readonly string thorn = "";
    private static readonly string speedShoot = "";
    private static readonly string reducePriceTower = "Reduces price all towers on 20 percent";
    private static readonly string increasePriceSell = "Increases price sell of tower on 50 percent";
}
