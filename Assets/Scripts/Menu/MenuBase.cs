using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MenuBase : MonoBehaviour
{
    public GameObject ThisGameObject { get => gameObject; }

    [HideInInspector]
    public GameObject enableObject;

    public void DisableAndEnableGameObject(GameObject disableObject, GameObject enableObject) {
        disableObject.gameObject.SetActive(false);
        enableObject.gameObject.SetActive(true);
    }

    public void DisableGameObject(GameObject _object) {
        _object.SetActive(false);
    }

    public void SetEnableObject(GameObject disableObject, GameObject enableObject) {
        MenuBase _menuWithButtonBack = enableObject.GetComponent<MenuBase>();

        if (_menuWithButtonBack != null) {
            _menuWithButtonBack.enableObject = disableObject;
        }
        else {
            Debug.LogError("object 'SettingsMenu' is null");
        }
    }

    public void LoadGameLevel(int level) {
        SceneManager.LoadScene(level);
    }

    public void StopTime() {
        Time.timeScale = 0f;
    }

    public void StartTime() {
        Time.timeScale = 1f;
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
