using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private int _coins;
    private int _currentHealth;
    private int _countStars;

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

    public EnemySpawner enemySpawner;
    public InformationPanel informationPanel;
    public GameMenu gameMenu;

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
        informationPanel.SetValueOnCointText(_coins);
        SetWaveText();
        informationPanel.SetHealthText(_health);
    }

    private void Update() {
        PauseGame();
    }

    private void PauseGame() {
        if (Input.GetKeyDown(_pauseButton) && !_isPause) {
            _isPause = !_isPause;
            StopTime();
            gameMenu.SetActiveBackgroundGameMenu(_isPause);
            gameMenu.SetActiveDisablePauseMenu(_isPause);
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
        informationPanel.SetValueOnCointText(_coins);
    }

    public void SubstractCoin(int amount) {
        _coins -= amount;
        informationPanel.SetValueOnCointText(_coins);
    }

    public void SetWaveText() {
        informationPanel.UpdateCountWaweText();
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
        gameMenu.EnableBackgroundGameMenu();
        gameMenu.EnableLoseMenu();
    }

    public void GameUnpause() {
        _isPause = false;
    }

    public void CheckLastEnemyAndEnableWinMenuOrSpawnNewEnemyWave() {
        if (enemySpawner.IsTheLastEnemyInTheLastWave) {
            SoundManager.Instance.PlaySound(SoundName.WinGame);
            gameMenu.EnableBackgroundGameMenu();
            gameMenu.EnableWinMenuAndSetDeafaultSpeedTime();
            _countStars = CalculationStars();
            gameMenu.WinMenu.amountReceivedStarsOnCurrentLevel = _countStars;
        }

        else if (enemySpawner.IsTheLastEnemyInCurrentWave) {
            enemySpawner.EnableTimerWave();
            enemySpawner.CalculationEnemyInCurrentWave();
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
        informationPanel.SetHealthText(_currentHealth);
    }

    public void GamePause() {
        _isPause = true;
    }
}
