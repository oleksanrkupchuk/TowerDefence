using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InformationPanel : MonoBehaviour {
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

    private string _valueTimeDoubletSpeed = "X2";

    [Header("Information objects")]
    [SerializeField]
    private InformationObject _coin;
    [SerializeField]
    private InformationObject _countWawe;
    [SerializeField]
    private InformationObject _health;

    [Header("Information parameters")]
    [SerializeField]
    private TextMeshProUGUI[] _priceTowerText;

    [Header("Game Manager")]
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TowerManager _towerManager;

    private void OnEnable() {
        DisableTimeSpeedText();
        DisableBackground();
        DisableButtonDefaultTimeSpeed();
    }

    private void Start() {
        //DisableButtonDefaultTimeSpeed();
        SetValueOnPriceTowerTextAndSubsñriptionButtonsTower();
        SubscriptionButton();
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
        priceText.text = "" + towerButton.TowerScript.Price;
    }

    private void SubscriptionTowerButtons(TowerButton towerButton) {
        towerButton.Button.onClick.AddListener(() => {
            _towerManager.SetSelectedTower(towerButton);
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

    public void SetValueOnCointText(string value) {
        _coin.textComponent.text = value;
    }

    public void SetValueInCountWaweText(string value) {
        _countWawe.textComponent.text = value;
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

    public void SetHealthText(int value) {
        _health.textComponent.text = "" + value;
    }
}
