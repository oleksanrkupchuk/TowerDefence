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
        UdpateCoin();
    }

    public void SubstractCoin(int amount) {
        _coin -= amount;
        UdpateCoin();
    }

    public void RewindingTime() {
        Time.timeScale = 2f;
    }

    public void DeafaultTime() {
        Time.timeScale = 1f;
    }

    private void UdpateCoin() {
        coinText.text = _coin.ToString();
    }
}
