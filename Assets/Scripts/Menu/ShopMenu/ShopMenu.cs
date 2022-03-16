using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopMenu : BaseMenu {
    private Ability _currentAbility;
    private int _amountAllStars;
    private AbilityPurchased _abilityPurchased;
    private List<Ability> _abilityes = new List<Ability>();
    private LevelData _levelData;

    [Header("Buttons Lose Menu")]
    [SerializeField]
    private Button _back;
    [SerializeField]
    private Button _buy;
    [SerializeField]
    private Button _yes;
    [SerializeField]
    private Button _no;
    [SerializeField]
    private Button _ok;

    [Header("Windows")]
    [SerializeField]
    private GameObject _confirmBuyAbilityWindow;
    [SerializeField]
    private GameObject _notEnoughMoneyWindow;

    [SerializeField]
    private Ability _ability;
    [SerializeField]
    private ApplyingAbility _applyingAbility;
    [SerializeField]
    private Scrollbar _scrollBar;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Image _detailAbilityIcon;
    [SerializeField]
    private TextMeshProUGUI _detailAbilityDescription;
    [SerializeField]
    private TextMeshProUGUI _starsText;
    [SerializeField]
    private List<AbilityData> _abilityData = new List<AbilityData>();

    public int AmountAbility { get => _abilityData.Count; }

    private void OnEnable() {
        _scrollBar.value = 0f;

        _detailAbilityIcon.sprite = _abilityData[0].icon;
        _detailAbilityDescription.text = _abilityData[0].description;

        LoadStars();
        #region FOR TEST DATA
        //LoadAbility();
        #endregion
    }

    private void LoadStars() {
        _levelData = SaveSystemLevel.LoadLevelData();
        _amountAllStars = _levelData.stars;
        _starsText.text = "" + _amountAllStars;
    }

    private void LoadAbility() {
        //print("fire ability exist = " + AbilitySaveSystem.IsExistsSaveAbilityFile());
        _abilityPurchased = AbilitySaveSystem.LoadAbility();
    }

    private void Start() {
        LoadAbility();
        InitAbilityData();
        SpawnAbility();
        DisableConfirmBuyAbilityWindow();
        DisableNotEnoughMoneyWindow();
        SubscriptionButtons();

        _detailAbilityIcon.sprite = _abilityData[0].icon;
        _detailAbilityDescription.text = _abilityData[0].description;

        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }

        DisableGameObject(gameObject);
    }

    private void InitAbilityData() {
        for (int i = 0; i < _abilityData.Count; i++) {
            for (int j = 0; j < _abilityData.Count; j++) {
                if (_abilityData[i].type == _abilityPurchased.abilities[j].type) {
                    _abilityData[i].isPurchased = _abilityPurchased.abilities[j].isPurchased;
                }
            }
        }
    }

    private void SpawnAbility() {
        for (int index = 0; index < _abilityData.Count; index++) {
            Ability _abilityObject = Instantiate(_ability);
            _abilityObject.transform.SetParent(_content);
            _abilityObject.transform.localScale = new Vector3(1f, 1f, 1f);

            _abilityObject.Init(_abilityData[index], this);
            _abilityObject.CheckForPurchasedAbilityAndSetIcon();
            _abilityes.Add(_abilityObject);
        }

        _currentAbility = _abilityes[0];
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });

        _buy.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            CheckMoneyAndEnableAbilityOrNotEnoughMoneyWindow();
        });

        _yes.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            SubstractPrice();
            SaveStars();
            SavePurchasedAbility();
            ApplyAbility();
            DisableConfirmBuyAbilityWindow();
        });

        _no.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableConfirmBuyAbilityWindow();
        });

        _ok.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableNotEnoughMoneyWindow();
        });
    }

    private void CheckMoneyAndEnableAbilityOrNotEnoughMoneyWindow() {
        if (_amountAllStars < _currentAbility.Data.price) {
            EnableNotEnoughMoneyWindow();
        }
        else {
            EnableConfirmBuyAbilityWindow();
        }
    }

    private void EnableNotEnoughMoneyWindow() {
        _notEnoughMoneyWindow.SetActive(true);
    }

    private void EnableConfirmBuyAbilityWindow() {
        _confirmBuyAbilityWindow.SetActive(true);
    }

    private void SubstractPrice() {
        _amountAllStars -= _currentAbility.Data.price;
        _starsText.text = "" + _amountAllStars;
    }

    private void SaveStars() {
        List<Level> _levels = _levelData.levels;
        SaveSystemLevel.SaveLevel(_amountAllStars, _levels);
    }

    private void SavePurchasedAbility() {
        _currentAbility.Data.isPurchased = true;
        _currentAbility.CheckForPurchasedAbilityAndSetIcon();

        _abilityPurchased = AbilitySaveSystem.LoadAbility();
        List<AbilityItem> _abilities = _abilityPurchased.abilities;

        for (int i = 0; i < _abilities.Count; i++) {
            if (_currentAbility.Data.type == _abilities[i].type) {
                _abilities[i].isPurchased = true;
            }
        }

        AbilitySaveSystem.SaveAbility(_abilities);
    }

    private void ApplyAbility() {
        _applyingAbility.ApplyAbility(_currentAbility.Data.type);
    }

    private void DisableConfirmBuyAbilityWindow() {
        _confirmBuyAbilityWindow.SetActive(false);
    }

    private void DisableNotEnoughMoneyWindow() {
        _notEnoughMoneyWindow.SetActive(false);
    }

    private void EnableBuyButton() {
        _buy.gameObject.SetActive(true);
    }

    private void DisableBuyButton() {
        _buy.gameObject.SetActive(false);
    }

    public void SetCurrentAbility(Ability ability) {
        _currentAbility = ability;
    }

    public void CheckPurchasedAbilityAndEnableOrDisableBuyButton() {
        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }
        else {
            EnableBuyButton();
        }
    }

    public void SetDetailsAbilityIcon(Sprite icon) {
        _detailAbilityIcon.sprite = icon;
    }

    public void SetDetailsAbilityDescription(string description) {
        _detailAbilityDescription.text = description;
    }
}
