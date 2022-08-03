using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

    private float _time = 5f;
    private float _timer;

    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Text _progressValue;
    [SerializeField]
    private LocalizeStringEvent _adviceLocalize;

    private List<string> _keysAdvice = new List<string> {
        "Key_Buy_Ability",
        "Key_Chest",
    };

    private void Start() {
        int _randomKeyAdvice = Random.Range(0, _keysAdvice.Count);
        LocalizedString _adviceLocalized = new LocalizedString { TableReference = "GameAdvices", TableEntryReference = _keysAdvice[_randomKeyAdvice] };
        _adviceLocalize.StringReference = _adviceLocalized;
        gameObject.SetActive(false);
    }

    public void LoadLevel(int sceneIndex) {
        StartCoroutine(LoadAsynchrosously(sceneIndex));
    }

    private IEnumerator LoadAsynchrosously(int sceneIndex) {

        while (_timer < _time) {
            _timer += Time.deltaTime;
            float _progress = Mathf.Clamp01(_timer / _time);
            float _progresseaseInCirc = 1 - Mathf.Sqrt(1 - Mathf.Pow(_progress, 2));
            _slider.value = _progresseaseInCirc;
            float _persentProgress = _progresseaseInCirc * 100;
            if (_persentProgress < 100) {
                _progressValue.text = "" + _persentProgress.ToString("00.00") + "%";
            }
            else {
                _progressValue.text = "" + _persentProgress.ToString("000.00") + "%";
            }

            yield return null;
        }

        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
