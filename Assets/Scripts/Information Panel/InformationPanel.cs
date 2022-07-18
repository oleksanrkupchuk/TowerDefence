using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization.Components;

public class InformationPanel : MonoBehaviour {
    private string _valueTimeDoubletSpeed = "X2";

    [SerializeField]
    private GameObject _backgroundMenu;
    [SerializeField]
    private Button _buttonDefaultTimeSpeed;
    [SerializeField]
    private Button _buttonDoubleTimeSpeed;
    [SerializeField]
    private TextMeshProUGUI _timeSpeedText;
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
        DisableTimeSpeedText();
        DisableBackground();
        DisableButtonDefaultTimeSpeed();
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

    private void DisableButtonDefaultTimeSpeed() {
        _buttonDefaultTimeSpeed.interactable = false;
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
        _buttonDefaultTimeSpeed.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            SetDeafaultTimeSpeed();
            EnableButtonDoubleTimeSpeed();
            DisableButtonDefaultTimeSpeed();
            DisableTimeSpeedText();
        });

        _buttonDoubleTimeSpeed.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            SetDoubleTimeSpeed();
            EnableButtonDefaultTimeSpeed();
            DisableButtonDoubleTimeSpeed();
            SetValueInTimeSpeedText(_valueTimeDoubletSpeed);
            EnableTimeSpeedText();
            StartCoroutine(AnimationForTimeSpeedText());
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
        _buttonDefaultTimeSpeed.interactable = true;
    }

    private void DisableButtonDoubleTimeSpeed() {
        _buttonDoubleTimeSpeed.interactable = false;
    }

    private void EnableButtonDoubleTimeSpeed() {
        _buttonDoubleTimeSpeed.interactable = true;
    }

    private void SetValueInTimeSpeedText(string value) {
        _timeSpeedText.text = value;
    }

    private void DisableTimeSpeedText() {
        _timeSpeedText.gameObject.SetActive(false);
    }

    private void EnableTimeSpeedText() {
        _timeSpeedText.gameObject.SetActive(true);
    }

    private IEnumerator AnimationForTimeSpeedText() {
        while (_timeSpeedText.gameObject.activeSelf == true) {
            ScaleGameObject(_timeSpeedText.gameObject, 1.2f, 1f);
            yield return new WaitForSeconds(1f);
            ScaleGameObject(_timeSpeedText.gameObject, 1f, 1f);
            yield return new WaitForSeconds(1f);
        }
    }

    private void ScaleGameObject(GameObject gameObject, float size, float time) {
        Vector3 scaletext = new Vector3(size, size);
        LeanTween.scale(gameObject, scaletext, time);
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
