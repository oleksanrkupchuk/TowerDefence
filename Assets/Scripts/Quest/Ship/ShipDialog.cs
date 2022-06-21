using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDialog : MonoBehaviour
{
    [SerializeField]
    private Ship _ship;
    [SerializeField]
    private Button _pay;
    [SerializeField]
    private Button _refuse;
    [SerializeField]
    private GameManager _gameManager;

    private void Awake() {
        _pay.onClick.AddListener(() => {
            CheckMoney();
        });
        _refuse.onClick.AddListener(() => {
            _ship.SetGoAwayPointAndMove();
            _ship.canBuyService = false;
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }

    private void CheckMoney() {
        if(_gameManager.Coins >= _ship.PriceForService) {
            _gameManager.SubstractCoin(_ship.PriceForService);
            //_ship.ChangeState(new ShipStateAttack());
            _ship.EnableSight();
            _ship.canBuyService = false;
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(false);
        }
    }

}
