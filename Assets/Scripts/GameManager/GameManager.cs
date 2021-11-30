using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] private int _coin;
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField]
    private GameObject _menuBackground;
    [SerializeField]
    private TextMeshProUGUI _waveText;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private InformationPanel _informationPanel;

    [Header("Menu")]
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _loseMenu;

    [SerializeField]
    private bool _isPause;
    private float _timer;
    [SerializeField]
    private float _time;
    [SerializeField]
    private int _health;

    public int Coin { get => _coin; }

    [SerializeField]
    private KeyCode _pauseButton;

    private void Start() {
        coinText.text = _coin.ToString();
        _waveText.text = "WAVE: " + 0;
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
            _pauseMenu.SetActive(_isPause);
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
        coinText.text = _coin.ToString();
    }

    public void UpdateWaveText(int countWave) {
        _waveText.text = "WAVE: " + countWave;
    }

    public void SetValueForTimer(float time) {
        _timer = time;
    }

    public float GetTimerValue() {
        return _timer;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null) {
            TakeAwayOneHealth();
            HealthCheckForZero();
        }
    }

    private void TakeAwayOneHealth() {
        _health--;
        UpdateHealthText();
    }

    private void UpdateHealthText() {
        _informationPanel.SetHealthText(_health);
    }

    private void HealthCheckForZero() {
        if (_health <= 0) {
            ShowLoseMenu();
            StopTime();
        }
    }

    private void ShowLoseMenu() {
        GameUnpause();
        _menuBackground.SetActive(true);
        _loseMenu.SetActive(true);
    }

    public void EnableTimerWaveAndSetValueForTimer() {
        if (_enemySpawner.IsTheLastEnemyInWave()) {
            _informationPanel.EnableTimerWaveObject();
            _informationPanel.StartAnimationForTimerWave();
            SetValueForTimer(_time);
        }
    }

    public void GamePause() {
        _isPause = true;
    }

    public void GameUnpause() {
        _isPause = false;
    }
}
