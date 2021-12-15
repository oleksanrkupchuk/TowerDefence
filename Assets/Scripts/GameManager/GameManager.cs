using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] private int _coin;

    [SerializeField]
    private GameObject _menuBackground;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private InformationPanel _informationPanel;

    [Header("Menu")]
    [SerializeField]
    private GameMenu _gameMenu;

    [SerializeField]
    private bool _isPause;
    private float _timer;
    [SerializeField]
    private float _time;
    [SerializeField]
    private int _health;

    private bool IsNotZeroHealth {
        get {
            if (_health > 0) {
                return true;
            }

            return false;
        }
    }

    public int Coin { get => _coin; }

    [SerializeField]
    private KeyCode _pauseButton;

    private void Start() {
        _informationPanel.SetValueOnCointText(_coin.ToString());
        _informationPanel.SetValueOnCountWaweText("WAVE: " + 0);
        SetValueForTimer(_time);
        _informationPanel.SetHealthText(_health);
    }

    private void Update() {
        PauseGame();
        CounterUntilWave();
    }

    private void CounterUntilWave() {
        if (_informationPanel.IsActiveCounter()) {
            if (_timer > 0) {

                _timer -= Time.deltaTime;
                _informationPanel.SetTimerText(_timer);
                _informationPanel.PlayAnimationForTimerIcon();

                if (_timer > 3f) {
                    _informationPanel.SetWhiteColorForTextTimerWave();
                }

                else if (_timer <= 3f) {
                    _informationPanel.SetRedColorForTextTimerWave();
                }
            }

            else if (_timer <= 0) {
                _enemySpawner.StartSpawn();
                _informationPanel.DisableTimerWaveObject();
            }
        }
    }

    private void PauseGame() {
        if (Input.GetKeyDown(_pauseButton) && !_isPause) {
            _isPause = !_isPause;
            StopTime();
            _menuBackground.SetActive(_isPause);
            _gameMenu.DisablePauseMenu();
        }
    }

    public void StartTime() {
        Time.timeScale = 1f;
    }

    private void StopTime() {
        Time.timeScale = 0f;
    }

    public void AddCoin(int amount) {
        _coin += amount;
        UpdateAmountCoin();
    }

    public void SubstractCoin(int amount) {
        _coin -= amount;
        UpdateAmountCoin();
    }

    public void UpdateAmountCoin() {
        _informationPanel.SetValueOnCointText(_coin.ToString());
    }

    public void UpdateWaveText(int countWave) {
        _informationPanel.SetValueOnCountWaweText("WAVE: " + countWave);
    }

    public void SetValueForTimer(float time) {
        _timer = time;
    }

    public float GetTimerValue() {
        return _timer;
    }

    public void CheckLastEnemyEnableTimerWaveAndSetValueForTimer() {
        if (_enemySpawner.IsTheLastEnemyInWave && !_enemySpawner.IsLastWave) {
            _informationPanel.EnableTimerWaveObject();
            _informationPanel.StartAnimationForTimerWave();
            SetValueForTimer(_time);
        }
    }

    public void LastEnemyEnableTimerWaveAndSetValueForTimer() {
        if (IsNotZeroHealth) {
            if (_enemySpawner.IsTheLastEnemyInWave && _enemySpawner.IsLastWave) {
                _menuBackground.SetActive(true);
                _gameMenu.EnableWinMenu();
                StopTime();
            }

            else if(_enemySpawner.IsTheLastEnemyInWave && !_enemySpawner.IsLastWave) {
                _informationPanel.EnableTimerWaveObject();
                _informationPanel.StartAnimationForTimerWave();
                SetValueForTimer(_time);
            }
        }

        else {
            ShowLoseMenu();
            StopTime();
        }
    }

    public void TakeAwayOneHealth() {
        _health--;
        UpdateHealthText();
    }

    private void UpdateHealthText() {
        _informationPanel.SetHealthText(_health);
    }

    private void ShowLoseMenu() {
        GameUnpause();
        _menuBackground.SetActive(true);
        _gameMenu.EnableLoseMenu();
    }

    public void GamePause() {
        _isPause = true;
    }

    public void GameUnpause() {
        _isPause = false;
    }
}
