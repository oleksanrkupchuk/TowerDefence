using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
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
            return towerObject.GetComponent<Tower>();
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
