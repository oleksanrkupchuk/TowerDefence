using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization.Components;

public class InformationPanel : MonoBehaviour {
    [SerializeField]
    private GameObject _backgroundMenu;
    [SerializeField]
    private Button _buttonSingleArrow;
    [SerializeField]
    private Button _buttonDoubleArrow;
    [SerializeField]
    private TowerButton[] _towerButton;

    [Header("Information objects")]
    [SerializeField]
    private Text _coins;
    [SerializeField]
    private Text _health;

    [Header("Information parameters")]
    [SerializeField]
    private TextMeshProUGUI[] _priceTowerText;


    [Header("String events")]
    [SerializeField]
    private LocalizeStringEvent _countWaveStringEvent;
    [SerializeField]
    private Transform _parentUnlockToolTipEnemy;
    [SerializeField]
    private UnlockEnemyToolTip _unlockToolTip;

    [Header("Managers")]
    public GameManager gameManager;
    public TowerManager towerManager;
    public EnemySpawner enemySpawner;

    private void OnEnable() {
        DisableBackground();
        NotInteractableButtonSingleArrow();
        UnlockEnemy.IsUnlockEnemy += SpawnUnlockToolTipEnemy;
    }

    private void SpawnUnlockToolTipEnemy() {
        UnlockEnemyToolTip _unlockToolTipObject = Instantiate(_unlockToolTip);
        _unlockToolTipObject.transform.SetParent(_parentUnlockToolTipEnemy);
        _unlockToolTipObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Start() {
        InitStringEvent();
        _countWaveStringEvent.gameObject.SetActive(true);
        SetValueOnPriceTowerTextAndSubsñriptionButtonsTower();
        SubscriptionButton();
    }

    private void InitStringEvent() {
        _countWaveStringEvent.StringReference.Arguments = new[] { enemySpawner };
    }

    private void NotInteractableButtonSingleArrow() {
        _buttonSingleArrow.interactable = false;
    }

    private void SetValueOnPriceTowerTextAndSubsñriptionButtonsTower() {
        for (int i = 0; i < _towerButton.Length; i++) {
            SetValueOnPriceTextTower(_priceTowerText[i], _towerButton[i]);
            SubscriptionTowerButtons(_towerButton[i]);
        }
    }

    private void SetValueOnPriceTextTower(TextMeshProUGUI priceText, TowerButton towerButton) {
        priceText.text = "" + towerButton.Tower.Price;
    }

    private void SubscriptionTowerButtons(TowerButton towerButton) {
        towerButton.Button.onClick.AddListener(() => {
            if(gameManager.Coins >= towerButton.Tower.Price) {
            towerManager.SetSelectedTower(towerButton);
            }
            else {
                SoundManager.Instance.PlaySoundEffect(SoundName.ErrorSetTower);
            }
        });
    }

    private void SubscriptionButton() {
        _buttonSingleArrow.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            SetDeafaultTimeSpeed();
            InteractableButtonDoubleArrow();
            NotInteractableButtonSingleArrow();
        });

        _buttonDoubleArrow.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            SetDoubleTimeSpeed();
            EnableButtonDefaultTimeSpeed();
            NotInteractableButtonDoubleArrow();
        });
    }

    private void DisableBackground() {
        _backgroundMenu.gameObject.SetActive(false);
    }

    public void SetValueOnCointText(int coins) {
        _coins.text = "" + coins;
    }

    private void SetDeafaultTimeSpeed() {
        Time.timeScale = 1f;
    }

    private void SetDoubleTimeSpeed() {
        Time.timeScale = 2f;
        //Time.timeScale = 0.2f;
    }

    private void EnableButtonDefaultTimeSpeed() {
        _buttonSingleArrow.interactable = true;
    }

    private void NotInteractableButtonDoubleArrow() {
        _buttonDoubleArrow.interactable = false;
    }

    private void InteractableButtonDoubleArrow() {
        _buttonDoubleArrow.interactable = true;
    }

    public void SetHealthText(int health) {
        _health.text = "" + health;
    }

    public void UpdateCountWaweText() {
        _countWaveStringEvent.RefreshString();
    }

    private void OnDestroy() {
        UnlockEnemy.IsUnlockEnemy -= SpawnUnlockToolTipEnemy;
    }
}
