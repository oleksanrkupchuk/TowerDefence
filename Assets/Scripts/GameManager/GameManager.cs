using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] 
    private int _coin;
    private int _currentHealth;
    private int _countStars;
    private float _timer;

    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private InformationPanel _informationPanel;
    [SerializeField]
    private GameMenu _gameMenu;

    [SerializeField]
    private KeyCode _pauseButton;
    [SerializeField]
    private bool _isPause;
    [SerializeField]
    private float _time;
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _leftThePercentageOfHealthToReceiveOneStar;
    [SerializeField]
    private int _leftThePercentageOfHealthToReceiveTwoStar;
    [SerializeField]
    private int _leftThePercentageOfHealthToReceiveThreeStar;
    public int Coin { get => _coin; }

    private int LeftPercentageOfHealth {
        get {
            return (_currentHealth * 100) / _health;
        }
    }

    private bool IsNotZeroHealth {
        get {
            if (_currentHealth > 0) {
                return true;
            }

            return false;
        }
    }

    private void Start() {
        _currentHealth = _health;
        _informationPanel.SetValueOnCointText(_coin.ToString());
        _informationPanel.SetValueOnCountWaweText("WAVE: " + 0);
        SetValueForTimer(_time);
        _informationPanel.SetHealthText(_health);
    }

    private void Update() {
        PauseGame();
        CounterUntilWave();
    }

    private void PauseGame() {
        if (Input.GetKeyDown(_pauseButton) && !_isPause) {
            _isPause = !_isPause;
            StopTime();
            _gameMenu.SetActiveBackgroundGameMenu(_isPause);
            _gameMenu.SetActiveDisablePauseMenu(_isPause);
        }
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
                _enemySpawner.EnemyWaveSpawn();
                _informationPanel.DisableTimerWaveObject();
            }
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

    private void UpdateAmountCoin() {
        _informationPanel.SetValueOnCointText(_coin.ToString());
    }

    public void UpdateWaveText(int countWave, int maxCountWawe) {
        _informationPanel.SetValueOnCountWaweText("WAVE: " + countWave + " / " + maxCountWawe);
    }

    public void SetValueForTimer(float time) {
        _timer = time;
    }

    public float GetTimerValue() {
        return _timer;
    }

    public void CheckHealthAndShowLoseMenuIfHealthZero() {
        if (IsNotZeroHealth) {
            CheckLastEnemyAndEnableWinMenu();
        }
        else {
            ShowLoseMenu();
            StopTime();
        }
    }

    public void CheckLastEnemyAndEnableWinMenu() {
        if (_enemySpawner.IsTheLastEnemyInTheLastWave) {
            _gameMenu.EnableBackgroundGameMenu();
            _gameMenu.EnableWinMenuAndSetDeafaultSpeedTime();
            _countStars = CalculationStars();
            _gameMenu.WinMenu.starsCurrentLevel = _countStars;
            _gameMenu.WinMenu.StartAnimationStars();
            //int _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            //SaveSystemStars.SaveStars(_countStars, _currentSceneIndex);
        }

        else if (_enemySpawner.IsTheLastEnemyInWave) {
            _informationPanel.EnableTimerWaveObject();
            _informationPanel.StartAnimationForTimerWave();
            SetValueForTimer(_time);
        }
    }

    public void TakeAwayOneHealth() {
        _currentHealth--;
        UpdateHealthText();
    }

    private void UpdateHealthText() {
        _informationPanel.SetHealthText(_currentHealth);
    }

    private void ShowLoseMenu() {
        GameUnpause();
        _gameMenu.EnableBackgroundGameMenu();
        _gameMenu.EnableLoseMenu();
    }

    public void GamePause() {
        _isPause = true;
    }

    public void GameUnpause() {
        _isPause = false;
    }

    private int CalculationStars() {
        //print("percent of health = " + LeftPercentageOfHealth);
        if(LeftPercentageOfHealth > _leftThePercentageOfHealthToReceiveThreeStar) {
            return 3;
        }
        if(LeftPercentageOfHealth > _leftThePercentageOfHealthToReceiveTwoStar) {
            return 2;
        }
        if(LeftPercentageOfHealth > _leftThePercentageOfHealthToReceiveOneStar) {
            return 1;
        }
        return 0;
    }
}
