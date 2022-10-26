using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class EnemyCart : MonoBehaviour
{
    private float _maxHealth = 50f;
    private float _maxSpeed = 1.5f;

    [SerializeField]
    private EnemyCartData _cartData;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Image _icon;
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
    private LocalizeStringEvent _localizeStringName;
    [SerializeField]
    private LocalizeStringEvent _localizeStringDescription;

    public void Init(EnemyCartData cartData, LocalizedString localizationStringName, LocalizedString localizationStringDescription) {
        _cartData = cartData;
        SetCharacteristics(localizationStringName, localizationStringDescription);
        CheckUnlockenemyAndHideCharacteristics();
    }

    private void SetCharacteristics(LocalizedString localizationStringName, LocalizedString localizationStringDescription) {
        _localizeStringName.StringReference = localizationStringName;
        _localizeStringDescription.StringReference = localizationStringDescription;
        SetHealthAndSpeed();
    }

    private void CheckUnlockenemyAndHideCharacteristics() {
        if (_cartData.isUnlockEnemy) {
            SetSizeRectTransform(_cartData.unlockEnemyIcon);
            _icon.sprite = _cartData.unlockEnemyIcon;
            SetctiveCharacteristics(true, false);
            SpawnDebuffs();
        }
        else {
            SetSizeRectTransform(_cartData.lockEnemyIcon);
            _icon.sprite = _cartData.lockEnemyIcon;
            SetctiveCharacteristics(false, true);
        }
    }

    private void SetSizeRectTransform(Sprite sprite) {
        _rectTransform.sizeDelta = new Vector2(sprite.bounds.size.x * 100, sprite.bounds.size.y * 100);
    }

    private void SpawnDebuffs() {
        for (int i = 0; i < _cartData.debuffs.Length; i++) {
            DebuffIcon _debuffIconObject = Instantiate(_debuffIcon);

            LocalizedString _tableDescription = new LocalizedString {
                TableReference = "Glossary",
                TableEntryReference = "Key_Debuff_" + _cartData.debuffs[i].typeDebuff.ToString()
            };

            _debuffIconObject.Init(_cartData.debuffs[i], _tableDescription);
            _debuffIconObject.transform.SetParent(_debuffsParent);
        }
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
