using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InformationPanel : MonoBehaviour
{
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
    private TimerWave _timerWave;
    [SerializeField]
    private EnemySpawner _enemySpawner;

    public float counter;

    private string _valueTimeDoubletSpeed = "X2";

    void Start()
    {
        DisableButtonDefaultTimeSpeed();
        SubscriptionButton();
    }

    private void SubscriptionButton() {
        _startButton.onClick.AddListener(() => {
            DisableStartButton();
            DisableBackground();
            EnableTimerObject();
            EnableTimerText();
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

    private void Update() {
        //StartCoroutine(CounterUntilWave());
        CounterUntilWave();
    }

    private void CounterUntilWave() {
        if (_timerWave.timerWaveText.enabled == true) {
            if (_enemySpawner.EnemyList.Count <= 0) {
                if (counter > 0) {
                    counter -= Time.deltaTime;
                    _timerWave.timerWaveText.text = counter.ToString("0.0");

                }

                else if (counter <= 0) {
                    _enemySpawner.StartSpawn();
                    _timerWave.timerObject.SetActive(false);
                }
            }
        }
    }

    private void DisableStartButton() {
        _startButton.gameObject.SetActive(false);
    }

    private void DisableBackground() {
        _backgroundMenu.gameObject.SetActive(false);
    }

    public void EnableTimerObject() {
        _timerWave.timerObject.SetActive(true);
    }

    private void DisableTimerObject() {
        _timerWave.timerObject.SetActive(false);
    }

    private void DisableTimerText() {
        _timerWave.timerWaveText.enabled = false;
    }

    private void EnableTimerText() {
        _timerWave.timerWaveText.enabled = true;
    }

    private void SetDeafaultTimeSpeed() {
        Time.timeScale = 1f;
    }

    private void SetDoubleTimeSpeed() {
        Time.timeScale = 2f;
    }

    private void DisableButtonDefaultTimeSpeed() {
        _buttonDefaultTimeSpeed.interactable = false;
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
            ScaleTimeSpeedText(1.2f, 1f);
            yield return new WaitForSeconds(1f);
            ScaleTimeSpeedText(1f, 1f);
            yield return new WaitForSeconds(1f);
        }
    }

    private void ScaleTimeSpeedText(float size, float time) {
        Vector3 scaletext = new Vector3(size, size);
        LeanTween.scale(_timeSpeedText.gameObject, scaletext, time);
    }
}
