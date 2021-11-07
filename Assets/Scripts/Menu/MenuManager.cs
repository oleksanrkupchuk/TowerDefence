using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum GameLevel {
    MainMenu,
    Level_1,
    Level_2,
    Level_3,
    None
}

public class MenuManager : MonoBehaviour {
    [SerializeField]
    private List<ButtonMenu> _buttonsMenu = new List<ButtonMenu>();

    private void Start() {
        SubscribeEvents();
    }

    private void SubscribeEvents() {
        for (int i = 0; i < _buttonsMenu.Count; i++) {
            GameObject _disableObject = _buttonsMenu[i].DisableObject;
            GameObject _enableObject = _buttonsMenu[i].enableObject;

            int level = _buttonsMenu[i].Level;

            switch (_buttonsMenu[i].TypeButtonMenu) {
                case TypeButtonMenu.ButtonSwapObjects:
                    _buttonsMenu[i].Button.onClick.AddListener(() => { SwapBetweenObjects(_disableObject, _enableObject); SetObjects(_disableObject, _enableObject); });
                    break;

                case TypeButtonMenu.ButtonLoadGameLevel:
                    _buttonsMenu[i].Button.onClick.AddListener(() => { LoadGameLevel(level); });
                    break;

                case TypeButtonMenu.ButtonQuit:
                    _buttonsMenu[i].Button.onClick.AddListener(() => { QuitApplication(); });
                    break;
            }
        }
    }

    private void LoadGameLevel(int level) {
        SceneManager.LoadScene(level);
    }

    private void SetObjects(GameObject disableObject, GameObject enableObject) {
        BackButton backButton = enableObject.GetComponent<BackButton>();

        if(backButton != null) {
            backButton.ButtonBack.enableObject = disableObject;
            backButton.ButtonBack.Button.onClick.AddListener(() => { SwapBetweenObjects(backButton.ButtonBack.DisableObject, backButton.ButtonBack.enableObject); });
        }
    }

    private void SwapBetweenObjects(GameObject disableObject, GameObject enableObject) {
        disableObject.gameObject.SetActive(false);
        enableObject.gameObject.SetActive(true);
    }

    private void QuitApplication() {
        Application.Quit();
    }
}
