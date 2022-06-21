using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopMenu : BaseMenu {
    private Ability _currentAbility;
    private int _stars;
    private AbilityPurchased _abilityPurchased;
    private List<Ability> _abilityes = new List<Ability>();
    private StarsData _starsData;

    [Header("Buttons Shop Menu")]
    [SerializeField]
    private Button _back;
    [SerializeField]
    private Button _buy;
    [SerializeField]
    private Button _ok;

    [Header("Windows")]
    [SerializeField]
    private ConfirmBuyAbilityWindow _confirmBuyAbilityWindow;
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
    private TextMeshProUGUI _starsText;
    [SerializeField]
    private List<AbilityData> _abilityData = new List<AbilityData>();

    [Header("Detail ability")]
    [SerializeField]
    private Image _abilityIcon;
    [SerializeField]
    private TextMeshProUGUI _abilityDescription;
    [SerializeField]
    private TextMeshProUGUI _abilityName;

    public int AmountAbility { get => _abilityData.Count; }
    public int PriceAbility { get => _currentAbility.Data.price; }

    private void OnEnable() {
        _scrollBar.value = 0f;

        SetInDetailFirsAbility();

        LoadStars();
        #region FOR TEST DATA
        //LoadAbility();
        #endregion
    }

    private void SetInDetailFirsAbility() {
        _abilityIcon.sprite = _abilityData[0].icon;
        _abilityDescription.text = _abilityData[0].description;
        _abilityName.text = _abilityData[0].name;
    }

    private void LoadStars() {
        _starsData = SaveAndLoadStars.LoadStars();
        _stars = _starsData.stars;
        _starsText.text = "" + _stars;
    }

    private void LoadAbility() {
        //print("fire ability exist = " + AbilitySaveSystem.IsExistsSaveAbilityFile());
        _abilityPurchased = AbilitySaveSystem.LoadAbility();
    }

    private void Start() {
        LoadAbility();
        InitAbilityData();
        SpawnAbility();
        _confirmBuyAbilityWindow.Disable();
        DisableNotEnoughMoneyWindow();
        SubscriptionButtons();

        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }

        DisableGameObject(gameObject);
    }

    private void InitAbilityData() {
        AbilityText.Init();
        for (int i = 0; i < _abilityData.Count; i++) {
            for (int j = 0; j < _abilityData.Count; j++) {
                if (_abilityData[i].type == _abilityPurchased.abilities[j].type) {
                    _abilityData[i].isPurchased = _abilityPurchased.abilities[j].isPurchased;
                    _abilityData[i].description = AbilityText.GetDescription(_abilityData[i].type);
                    //print("desc = " + AbilityText.GetDescription(_abilityData[i].type));
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

        _confirmBuyAbilityWindow.yes.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            SubstractPrice();
            SaveStars();
            SavePurchasedAbility();
            ApplyAbility();
            _confirmBuyAbilityWindow.Disable();
        });

        _confirmBuyAbilityWindow.no.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            _confirmBuyAbilityWindow.Disable();
        });

        _ok.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableNotEnoughMoneyWindow();
        });
    }

    private void CheckMoneyAndEnableAbilityOrNotEnoughMoneyWindow() {
        if (_stars < _currentAbility.Data.price) {
            EnableNotEnoughMoneyWindow();
        }
        else {
            _confirmBuyAbilityWindow.Enable();
        }
    }

    private void EnableNotEnoughMoneyWindow() {
        _notEnoughMoneyWindow.SetActive(true);
    }

    public void SubstractPrice() {
        _stars -= _currentAbility.Data.price;
        _starsText.text = "" + _stars;
    }

    public void SaveStars() {
        _starsData.stars = _stars;
        SaveAndLoadStars.SaveStars(_starsData);
    }

    public void SavePurchasedAbility() {
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

    public void ApplyAbility() {
        _applyingAbility.ApplyAbility(_currentAbility.Data.type);
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

    public void SetCurrentAbilityAndUpdateDetails(Ability ability) {
        _currentAbility = ability;
        _abilityIcon.sprite = _currentAbility.Data.icon;
        _abilityName.text = _currentAbility.Data.name;
        _abilityDescription.text = _currentAbility.Data.description;
    }

    public void CheckPurchasedAbilityAndEnableOrDisableBuyButton() {
        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }
        else {
            EnableBuyButton();
        }
    }
}
