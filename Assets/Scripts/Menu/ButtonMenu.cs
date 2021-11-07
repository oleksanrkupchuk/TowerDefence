using UnityEngine;
using UnityEngine.UI;

public enum TypeButtonMenu {
    ButtonSwapObjects,
    ButtonLoadGameLevel,
    ButtonQuit,
    None
}

public class ButtonMenu : MonoBehaviour
{
    [SerializeField]
    private TypeButtonMenu _typeButtonMenu = new TypeButtonMenu();
    public TypeButtonMenu TypeButtonMenu { get => _typeButtonMenu; }

    public GameObject enableObject;
   
    [SerializeField]
    private GameObject _disableObject;
    public GameObject DisableObject { get => _disableObject; set => _disableObject = value; }

    [SerializeField]
    private Button _button;
    public Button Button { get => _button; }

    [SerializeField]
    private GameLevel _level = new GameLevel();
    public int Level { get => (int)_level; }
}
