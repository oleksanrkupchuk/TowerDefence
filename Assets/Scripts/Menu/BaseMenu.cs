using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseMenu : MonoBehaviour
{
    private int _indexMainMenu = 0;
    public int IndexMainMenu { get => _indexMainMenu; }

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
        BaseMenu _menuWithButtonBack = enableObject.GetComponent<BaseMenu>();

        if (_menuWithButtonBack != null) {
            _menuWithButtonBack.enableObject = disableObject;
        }
        else {
            Debug.LogError("object without button 'Back'");
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
