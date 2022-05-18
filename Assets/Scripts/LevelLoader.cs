using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour {
    [SerializeField]
    private Slider _slider;
    //[SerializeField]
    //private TextMeshProUGUI _progressValue;
    [SerializeField]
    private Text _progressValue;

    private void Start() {
        gameObject.SetActive(false);
    }

    public void LoadLevel(int sceneIndex) {
        StartCoroutine(LoadAsynchrosously(sceneIndex));
    }

    private IEnumerator LoadAsynchrosously(int sceneIndex) {
        AsyncOperation _operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!_operation.isDone) {
            float _progress = Mathf.Clamp01(_operation.progress / .9f);
            _slider.value = _progress;
            _progressValue.text = "" + _progress * 100 + "%";

            yield return null;
        }
    }
}
