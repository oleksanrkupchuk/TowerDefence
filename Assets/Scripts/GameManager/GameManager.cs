using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Loader<GameManager>
{
    [SerializeField] private int coin;
    [SerializeField] private TextMeshProUGUI coinText;

    public int Coin
    {
        get
        {
            return coin;
        }
    }

    void Start()
    {
        coinText.text = coin.ToString();
    }

    void Update()
    {

    }
}
