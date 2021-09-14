using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    [SerializeField]
    private Button _buttonUpgrade;
    [SerializeField]
    private Button _buttonSell;
    [SerializeField]
    private Button _buttonRepair;

    [SerializeField]
    private Tower _tower;

    [SerializeField]
    private float _additionRangeRadius;

    private readonly string _upgrade = "upgrade";
    private readonly string _sell = "sell";
    private readonly string _repair = "repair";

    public Dictionary<string, int> ability = new Dictionary<string, int>();

    private void Awake() {
        InitializationDictionary();
    }

    private void InitializationDictionary() {
        ability[_upgrade] = 0;
        ability[_sell] = 0;
        ability[_repair] = 0;
    }

    private void Start()
    {
        _buttonUpgrade.onClick.AddListener(() => CheckUpgrade());
    }

    private void CheckUpgrade() {
        if(ability[_upgrade] < _tower.numberUpgrade) {
            _tower.UpgradeTower(_additionRangeRadius);
            ability[_upgrade] += 1;
        }
        else {
            print("max number upgrade");
        }
    }
}
