using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InformationPanel : MonoBehaviour {
    [SerializeField]
    private Button _startButton;
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
    private InformationObject _timerWave;
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
        DisableTimerWaveObject();
        DisableTimeSpeedText();
    }

    private void Start() {
        DisableButtonDefaultTimeSpeed();
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
            _towerManager.SelectedTower(towerButton);
        });
    }

    private void SubscriptionButton() {
        _startButton.onClick.AddListener(() => {
            _gameManager.StartTime();
            _gameManager.GameUnpause();
            DisableStartButton();
            DisableBackground();
            EnableTimerWaveObject();
            StartCoroutine(AnimationForTimerWave());
        });

        _buttonDefaultTimeSpeed.onClick.AddListener(() => {
            SetDeafaultTimeSpeed();
            EnableButtonDoubleTimeSpeed();
            DisableButtonDefaultTimeSpeed();
            DisableTimeSpeedText();
        });

        _buttonDoubleTimeSpeed.onClick.AddListener(() => {
            SetDoubleTimeSpeed();
            EnableButtonDefaultTimeSpeed();
            DisableButtonDoubleTimeSpeed();
            SetValueInTimeSpeedText(_valueTimeDoubletSpeed);
            EnableTimeSpeedText();
            StartCoroutine(AnimationForTimeSpeedText());
        });
    }

    public void SetTimerText(float time) {
        _timerWave.textComponent.text = time.ToString("0.0");
    }

    public bool IsActiveCounter() {
        if (_timerWave.objectComponent.activeSelf == true) {
            return true;
        }

        return false;
    }

    private void DisableStartButton() {
        _startButton.gameObject.SetActive(false);
    }

    private void DisableBackground() {
        _backgroundMenu.gameObject.SetActive(false);
    }

    public void SetValueOnCointText(string value) {
        _coin.textComponent.text = value;
    }

    public void SetValueOnCountWaweText(string value) {
        _countWawe.textComponent.text = value;
    }

    public void EnableTimerWaveObject() {
        _timerWave.objectComponent.SetActive(true);
    }

    public void DisableTimerWaveObject() {
        _timerWave.objectComponent.SetActive(false);
    }

    private void DisableTimerWaveText() {
        _timerWave.textComponent.enabled = false;
    }

    public void EnableTimerWaveText() {
        _timerWave.textComponent.enabled = true;
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

    public void StartAnimationForTimerWave() {
        StartCoroutine(AnimationForTimerWave());
    }

    private IEnumerator AnimationForTimerWave() {
        float _time = 0.5f;
        while (_timerWave.gameObject.activeSelf == true) {
            ScaleGameObject(_timerWave.objectComponent, 1.2f, _time);
            yield return new WaitForSeconds(_time);
            ScaleGameObject(_timerWave.objectComponent, 1f, _time);
            yield return new WaitForSeconds(_time);
        }
    }

    public void SetRedColorForTextTimerWave() {
        Color _redColor = Color.red;
        _timerWave.textComponent.color = _redColor;
    }

    public void SetWhiteColorForTextTimerWave() {
        Color _whiteColor = Color.white;
        _timerWave.textComponent.color = _whiteColor;
    }

    public void PlayAnimationForTimerIcon() {
        float amount = Mathf.Abs(_gameManager.GetTimerValue() - 5f) / 5f;
        _timerWave.iconComponent.fillAmount = amount;
    }

    private void ScaleGameObject(GameObject gameObject, float size, float time) {
        Vector3 scaletext = new Vector3(size, size);
        LeanTween.scale(gameObject, scaletext, time);
    }

    public void SetHealthText(int value) {
        _health.textComponent.text = "" + value;
    }
}
