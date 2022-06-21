using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DataTest : MonoBehaviour {
    [SerializeField]
    private Button _dataButton;
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private Button _resetAllData;
    [SerializeField]
    private Button _addStars;
    [SerializeField]
    private GameObject _dataWindow;

    private void Start() {
        DisableDataWindow();

        _dataButton.onClick.AddListener(() => {
            EnableDataWindow();
        });

        _backButton.onClick.AddListener(() => {
            DisableDataWindow();
        });

        _resetAllData.onClick.AddListener(() => {
            ResetAllData();
        });

        _addStars.onClick.AddListener(() => {
            AddStars();
        });
    }

    private void EnableDataWindow() {
        _dataWindow.SetActive(true);
    }

    private void DisableDataWindow() {
        _dataWindow.SetActive(false);
    }

    private void ResetAllData() {
        //LevelData _startData = SaveSystemLevel.LoadLevelData();
        //for (int i = 0; i < _startData.levels.Count; i++) {
        //}
        //LevelData _startData2 = SaveSystemLevel.LoadLevelData();
        ////for (int i = 0; i < _startData2.level.Length; i++) {
        ////    print($"stars on {i + 1} level = " + _startData2.level[i]);
        ////}
        //if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
        //    AbilityPurchased _abilityBurchaseData = AbilitySaveSystem.LoadAbility();
        //    //print($"ability load,  count = " + _abilityBurchaseData.abilities.Count);
        //    int count = _abilityBurchaseData.abilities.Count;
        //    for (int i = (count - 1); i < count && i >= 0; i--) {
        //        _abilityBurchaseData.abilities.RemoveAt(i);
        //    }
        //    AbilitySaveSystem.SaveAbility(_abilityBurchaseData.abilities);

        //    AbilityPurchased _abilityBurchaseData2 = AbilitySaveSystem.LoadAbility();
        //    if (_abilityBurchaseData2.abilities != null) {
        //        print("---------------------DATA TEST-------------------------");
        //        print($"ability count = " + _abilityBurchaseData2.abilities.Count);
        //        for (int i = 0; i < _abilityBurchaseData2.abilities.Count; i++) {
        //            print($"ability type = " + _abilityBurchaseData2.abilities[i].type + " purchased = " + _abilityBurchaseData2.abilities[i].isPurchased);
        //        }
        //    }
        //}
    }

    private void AddStars() {
        //LevelData _levelData = SaveSystemLevel.LoadLevelData();
        //List<Level> _levels = _levelData.levels;
        //SaveSystemLevel.SaveLevel(50, _levels);
    }
}
