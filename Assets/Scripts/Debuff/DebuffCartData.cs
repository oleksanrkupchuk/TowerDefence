using UnityEngine;

public enum Debuff {
    Burning,
    Explosion,
    IncreasedMove,
    Healing,
    SlowedMove,
}

[CreateAssetMenu(fileName = "DebuffCartData", menuName = "ScriptableObjects/DebuffCartData", order = 3)]
public class DebuffCartData : ScriptableObject
{
    public Debuff typeDebuff = new Debuff();
    public Sprite icon;
}
