using System.Collections.Generic;

public class AbilityText 
{
    private static Dictionary<AbilityType, string> _descriptions = new Dictionary<AbilityType, string>();

    public static void Init() {
        _descriptions.Clear();
        _descriptions.Add(AbilityType.SpeedShoot, speedShoot);
        _descriptions.Add(AbilityType.Thorn, thorn);
        _descriptions.Add(AbilityType.Burning, burning);
        _descriptions.Add(AbilityType.FireArea, fireArea);
        _descriptions.Add(AbilityType.Explosion, explosion);
        _descriptions.Add(AbilityType.ReducePriceTower, reducePriceTower);
        _descriptions.Add(AbilityType.IncreasePriceSell, increasePriceSell);
    }

    public static string GetDescription(AbilityType type) {
        return _descriptions[type];
    }

    private static readonly string fireArea = "When bullet of fire tower touch earth is being created circle of fire, chance creation circle of fire have 40 percent. Enemies which cross circle of fire burning some time";
    private static readonly string explosion = "When bullet of rock tower touch earth happens very strong explosion, which give damage surrounding enemies";
    private static readonly string burning = "Bullet of fire tower sets fire to enemy when touch his, enemy burning for some time";
    private static readonly string thorn = "Bullet of iron tower slow enemy for some time";
    private static readonly string speedShoot = "Iron tower shoots twice speed";
    private static readonly string reducePriceTower = "Reduces price all towers on 30 percent";
    private static readonly string increasePriceSell = "Increases price sell of tower to 80 percent";
}
