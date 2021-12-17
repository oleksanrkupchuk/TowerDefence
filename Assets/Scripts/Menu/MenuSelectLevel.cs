using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSelectLevel : MenuBase
{
    [Header("Buttons Pause Menu")]
    [SerializeField]
    private Button _back;

    [Header("Game objects")]
    [SerializeField]
    private GameObject _containerLvel;
    [SerializeField]
    private GameObject _level;

    [Header("Parametrs")]
    [SerializeField]
    private int _countLevel;

    private void Start() {
        SubscriptionButtons();
        SpawnLevelContainer();
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, enableObject); 
        });
    }

    private void SpawnLevelContainer() {
        for (int indexLevel = 1; indexLevel <= _countLevel; indexLevel++) {
            GameObject _levelObject = Instantiate(_level, _containerLvel.transform);
            LevelCart _levelCart = _levelObject.GetComponent<LevelCart>();

            SubscriptionLevelButton(_levelCart.button, indexLevel);
            SetTextOnLevelButton(_levelCart.textButton, indexLevel);
        }
    }

    private void SubscriptionLevelButton(Button levelButton, int indexLevel) {
        levelButton.onClick.AddListener(() => { 
            LoadGameLevel(indexLevel);
        });
    }

    private void SetTextOnLevelButton(TextMeshProUGUI buttonText, int indexLevel) {
        buttonText.text = "Level " + indexLevel;
    }
}
