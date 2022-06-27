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
    [SerializeField]
    private SettingsMenu _settingsMenu;

    private void OnEnable() {
        CreateStarsFile();
        CreateLevelFileAndInitLevels();

        if (!AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            CreateAbilityFileAndInitAbilityData();
        }

        StartCoroutine(CreateSettingsFileAndInitSettingsData());

        if (!SaveAndLoadEnemyCart.IsExistsSaveEnemyCartFile()) {
            CreateCartEnemies();
        }
    }

    private void CreateStarsFile() {
        if (SaveAndLoadStars.IsExistsStarsFile()) {
            return;
        }

        StarsData _starsData = new StarsData();
        _starsData.stars = 0;

        SaveAndLoadStars.SaveStars(_starsData);
    }

    private void CreateLevelFileAndInitLevels() {
        if (SaveSystemLevel.IsExistsLevelsFile()) {
            return;
        }

        List<Level> _levels = new List<Level>();

        for (int i = 0; i < _menuSelectLevel.AmountLevel; i++) {
            Level _level;

            if (i == 0) {
                _level = new Level();
                _level.levelIndex = i;
                _level.stars = 0;
                _level.isUnlock = true;
            }
            else {
                _level = new Level();
                _level.levelIndex = i;
                _level.stars = 0;
                _level.isUnlock = false;
            }

            _levels.Add(_level);
        }

        SaveSystemLevel.SaveLevels(_levels);
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

    private IEnumerator CreateSettingsFileAndInitSettingsData() {
        SettingsData _settingsData;

        if (!SaveSystemSettings.IsExistsSaveSettingsFile()) {
            _settingsData = new SettingsData();
            _settingsData.soundVolume = 1f;
            _settingsData.effectVolume = 1f;
            _settingsData.indexResolution = 0;
            _settingsData.indexLanguage = 0;
            _settingsData.fullScreenToggle = true;

            SaveSystemSettings.SaveSettings(_settingsData);
        }
        else {
            _settingsData = SaveSystemSettings.LoadSettings();

        }

        yield return StartCoroutine(_settingsMenu.LoadLanguages());
        _settingsMenu.LocaleSelected(_settingsData.indexLanguage);

        //print("volume = " + _settingsData.soundVolume);
        //print("resolution = " + _settingsData.indexResolution);
        //print("language = " + _settingsData.indexLanguage);
        //print("fullScreen = " + _settingsData.fullScreenToggle);
    }

    private void CreateCartEnemies() {
        List<bool> _isUnlocksEnemy = new List<bool>();
        for (int i = 0; i < _gameInformation.AmountEnemyCartData; i++) {
            _isUnlocksEnemy.Add(false);
        }

        SaveAndLoadEnemyCart.SaveEnemyCart(_isUnlocksEnemy);
    }
}
