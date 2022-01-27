using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private Tower _tower;
    [SerializeField] 
    private GameObject towerObject;
    [SerializeField] 
    private Sprite towerSprite;
    [SerializeField]
    private Button _button;

    public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }

    public Tower TowerScript {
        get {
            return _tower;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return towerSprite;
        }
    }

    public Button Button { get => _button; }
}
