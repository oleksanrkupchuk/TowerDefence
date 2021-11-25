using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Loader<GameManager>
{
    [SerializeField] private int _coin;
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField]
    private GameObject _menuBackground;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private TextMeshProUGUI _waveText;

    [SerializeField]
    private bool _isPause = false;

    public int Coin { get => _coin; }

    [SerializeField]
    private KeyCode _pauseButton;

    private void Start()
    {
        coinText.text = _coin.ToString();
        _waveText.text = "WAVE: " + 0;
    }

    private void Update() {
        if (Input.GetKeyDown(_pauseButton)) {
            _isPause = !_isPause;
            StopTime();
            _menuBackground.SetActive(_isPause);
            _pauseMenu.SetActive(_isPause);
        }
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

    public void ChangePause() {
        _isPause = !_isPause;
    }

    public void UpdateWaveText(int countWave) {
        _waveText.text = "WAVE: " + countWave;
    }
}
