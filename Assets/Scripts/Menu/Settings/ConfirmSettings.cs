using UnityEngine;
using UnityEngine.UI;

public class ConfirmSettings : BaseMenu
{
    [SerializeField]
    private Button _yes;
    [SerializeField]
    private Button _no;

    public Button Yes { get => _yes; }
    public Button No { get => _no; }
}
