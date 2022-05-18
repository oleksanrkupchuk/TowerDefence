using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData : MonoBehaviour {
    [SerializeField]
    private GameInformation _gameInformation;
    [SerializeField]
    private MenuSelectLevel _menuSelectLevel;
    [SerializeField]
    private ShopMenu _shopMenu;

    private void OnEnable() {
        if (!SaveSystemLevel.IsExistsSaveLevelsFile()) {
            InitDataLevelsAndCreateFile();
        }

        if (!AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            CreateAbilityFileAndInitAbilityData();
        }

        if (!SaveSystemSettings.IsExistsSaveSoundFile()) {
            CreateSoundFile();
        }

        if (!SaveAndLoadEnemyCart.IsExistsSaveEnemyCartFile()) {
            CreateCartEnemies();
        }
    }

    private void InitDataLevelsAndCreateFile() {
        List<Level> _levels = new List<Level>();
        Level _level;

        for (int i = 0; i < _menuSelectLevel.AmountLevel; i++) {
            if (i == 0) {
                _level = new Level(i, 0, true);
                _levels.Add(_level);
            }
            else {
                _level = new Level(i, 0, false);
                _levels.Add(_level);
            }
        }

        SaveSystemLevel.SaveLevel(0, _levels);
    }

    private void CreateAbilityFileAndInitAbilityData() {
        List<AbilityItem> _abilities = new List<AbilityItem>();
        AbilityItem _abilityItem;

        for (int i = 0; i < _shopMenu.AmountAbility; i++) {
            _abilityItem = new AbilityItem((AbilityType)i, false);
            _abilities.Add(_abilityItem);
        }

        AbilitySaveSystem.SaveAbility(_abilities);
    }

    private void CreateSoundFile() {
        SaveSystemSettings.SaveSoundData(100f);
        //SaveSoundData _data = SaveSystemSettings.LoadSound();
        //print("volume = " + _data.volume);
    }

    private void CreateSettingsFileAndInitSettingsData() {
        if (!SaveSystemSettings.IsExistsSaveSettingsFile()) {
            SettingsData _settingsData = new SettingsData();
            _settingsData.soundVolume = 100f;
            _settingsData.indexResolution = 0;
            _settingsData.fullScreenToggle = true;

            //SaveSystemSettings.SaveSettings(_settingsData);
        }

    }

    private void CreateCartEnemies() {
        List<bool> _isUnlocksEnemy = new List<bool>();
        for (int i = 0; i < _gameInformation.EnemyCartData; i++) {
            _isUnlocksEnemy.Add(false);
        }

        SaveAndLoadEnemyCart.SaveEnemyCart(_isUnlocksEnemy);
    }
}
