using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCart : MonoBehaviour
{
    private float _maxHealth = 50f;
    private float _maxSpeed = 1.5f;

    [SerializeField]
    private EnemyCartData _cartData;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;
    [SerializeField]
    private Slider _health;
    [SerializeField]
    private Slider _speed;
    [SerializeField]
    private GameObject _hideCharacteristics;
    [SerializeField]
    private GameObject _unlockCharacteristics;
    [SerializeField]
    private Transform _debuffsParent;
    [SerializeField]
    private DebuffIcon _debuffIcon;
    [SerializeField]
    private HorizontalLayoutGroup _horizontalLayoutGroup;

    public void Init(EnemyCartData cartData) {
        _cartData = cartData;
        CheckUnlockenemyAndHideCharacteristics();
    }

    private void CheckUnlockenemyAndHideCharacteristics() {
        SetCharacteristics();

        if (_cartData.unlockEnemy) {
            _icon.sprite = _cartData.unlockEnemyIcon;
            SetctiveCharacteristics(true, false);
            SpawnDebuffs();
            SetWeidthForParentDebuff();
        }
        else {
            _icon.sprite = _cartData.lockEnemyIcon;
            SetctiveCharacteristics(false, true);
        }
    }

    private void SpawnDebuffs() {
        for (int i = 0; i < _cartData.debuffs.Length; i++) {
            DebuffIcon _debuffIconObject = Instantiate(_debuffIcon);
            _debuffIconObject.Init(_cartData.debuffs[i]);
            _debuffIconObject.transform.SetParent(_debuffsParent);
        }
    }

    private void SetWeidthForParentDebuff() {
        int _countDebuff = _cartData.debuffs.Length;
        float _parentWidth = (_debuffIcon.Width * _countDebuff) + 
            _horizontalLayoutGroup.padding.left + _horizontalLayoutGroup.padding.right +
            (_horizontalLayoutGroup.spacing * (_countDebuff - 1));

        _debuffsParent.GetComponent<RectTransform>().sizeDelta = new Vector2(_parentWidth, 100f);
    }

    private void SetCharacteristics() {
        _title.text = _cartData.name;
        _description.text = _cartData.description;
        SetHealthAndSpeed();
    }

    private void SetctiveCharacteristics(bool showCharacteristics, bool hideCharacteristics) {
        _unlockCharacteristics.SetActive(showCharacteristics);
        _hideCharacteristics.SetActive(hideCharacteristics);
    }

    private void SetHealthAndSpeed() {
        _health.value = _cartData.enemy.Health / _maxHealth;
        _speed.value = _cartData.enemy.Speed / _maxSpeed;
    }
}
