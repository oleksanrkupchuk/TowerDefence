using UnityEngine;
using TMPro;

public class GameManager : Loader<GameManager>
{
    [SerializeField] private int _coin;
    [SerializeField] private TextMeshProUGUI coinText;

    public int Coin { get => _coin; }

    private void Start()
    {
        coinText.text = _coin.ToString();
    }

    public void AddCoin(int amount) {
        _coin += amount;
        UpdateCoin();
    }

    public void SubstractCoin(int amount) {
        _coin -= amount;
        UpdateCoin();
    }

    public void RewindingTime() {
        Time.timeScale = 2f;
    }

    public void DeafaultTime() {
        Time.timeScale = 1f;
    }

    public void UpdateCoin() {
        coinText.text = _coin.ToString();
    }
}
