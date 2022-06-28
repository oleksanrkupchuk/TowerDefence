using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private int _coins;
    private int _currentHealth;
    private int _countStars;

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
    private int _health;
    [SerializeField]
    private int _leftThePercentageOfHealthToReceiveOneStar;
    [SerializeField]
    private int _leftThePercentageOfHealthToReceiveTwoStar;
    [SerializeField]
    private int _leftThePercentageOfHealthToReceiveThreeStar;

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
    public int Coins { get => _coins; }

    private void Start() {
        _currentHealth = _health;
        _informationPanel.SetValueOnCointText(_coins);
        SetWaveText();
        _informationPanel.SetHealthText(_health);
    }

    private void Update() {
        PauseGame();
    }

    private void PauseGame() {
        if (Input.GetKeyDown(_pauseButton) && !_isPause) {
            _isPause = !_isPause;
            StopTime();
            _gameMenu.SetActiveBackgroundGameMenu(_isPause);
            _gameMenu.SetActiveDisablePauseMenu(_isPause);
        }
    }

    public void StartTime() {
        Time.timeScale = 1f;
    }

    private void StopTime() {
        Time.timeScale = 0f;
    }

    public void AddCoin(int amount) {
        _coins += amount;
        _informationPanel.SetValueOnCointText(_coins);
    }

    public void SubstractCoin(int amount) {
        _coins -= amount;
        _informationPanel.SetValueOnCointText(_coins);
    }

    public void SetWaveText() {
        _informationPanel.UpdateCountWaweText();
    }

    public void CheckHealthAndShowLoseMenuIfHealthZero() {
        if (IsNotZeroHealth) {
            CheckLastEnemyAndEnableWinMenuOrSpawnNewEnemyWave();
        }
        else {
            SoundManager.Instance.PlaySound(SoundName.LoseGame);
            ShowLoseMenu();
            StopTime();
        }
    }

    private void ShowLoseMenu() {
        GameUnpause();
        _gameMenu.EnableBackgroundGameMenu();
        _gameMenu.EnableLoseMenu();
    }

    public void GameUnpause() {
        _isPause = false;
    }

    public void CheckLastEnemyAndEnableWinMenuOrSpawnNewEnemyWave() {
        if (_enemySpawner.IsTheLastEnemyInTheLastWave) {
            SoundManager.Instance.PlaySound(SoundName.WinGame);
            _gameMenu.EnableBackgroundGameMenu();
            _gameMenu.EnableWinMenuAndSetDeafaultSpeedTime();
            _countStars = CalculationStars();
            _gameMenu.WinMenu.amountReceivedStarsOnCurrentLevel = _countStars;
            _gameMenu.WinMenu.StartAnimationStars();
        }

        else if (_enemySpawner.IsTheLastEnemyInCurrentWave) {
            _enemySpawner.EnableTimerWave();
            _enemySpawner.CalculationEnemyInCurrentWave();
        }
    }

    private int CalculationStars() {
        if (LeftPercentageOfHealth > _leftThePercentageOfHealthToReceiveThreeStar) {
            return 3;
        }
        if (LeftPercentageOfHealth > _leftThePercentageOfHealthToReceiveTwoStar) {
            return 2;
        }
        if (LeftPercentageOfHealth > _leftThePercentageOfHealthToReceiveOneStar) {
            return 1;
        }
        return 0;
    }

    public void TakeAwayOneHealth() {
        _currentHealth--;
        _informationPanel.SetHealthText(_currentHealth);
    }

    public void GamePause() {
        _isPause = true;
    }
}
