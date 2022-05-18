using UnityEngine;

[CreateAssetMenu(fileName = "DebuffCartData", menuName = "ScriptableObjects/DebuffCartData", order = 3)]
public class DebuffCartData : ScriptableObject
{
    public Sprite icon;
    public string title;
    [TextArea(5, 15)]
    public string description;
}
