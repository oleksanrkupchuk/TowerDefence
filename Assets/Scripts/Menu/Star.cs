using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour {
    private float _timer;
    private float _time;
    private bool _isBlink = false;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private float _timeFillingAnimation;
    [SerializeField]
    private float _timeDelayForShowFillingAnimation;
    [SerializeField]
    private float _timeBlinkAnimation;
    [SerializeField]
    private float _timeBlink;
    [SerializeField]
    private Color _colorBlink;
    [SerializeField]
    private Color _colorDefault;

    public bool endFillingAnimation;

    void Start() {
        _icon.fillAmount = 0;
        _timer = _timeFillingAnimation;
    }

    private void Update() {
        if (_isBlink) {
            _timeBlinkAnimation -= Time.deltaTime;

            if (_timeBlinkAnimation < 0) {
                _isBlink = false;
            }
        }
    }

    public void StartFillingAnimation() {
        StartCoroutine(FillingAnimation());
    }

    private IEnumerator FillingAnimation() {
        yield return new WaitForSeconds(_timeDelayForShowFillingAnimation);

        while (_timer >= 0) {
            _time += Time.deltaTime;
            _timer -= _time * _time;

            float _amount = Mathf.Abs(_timer - _timeFillingAnimation) / _timeFillingAnimation;
            _icon.fillAmount = _amount;

            yield return null;
        }

        endFillingAnimation = true;
    }

    public void StartBlinkAnimation() {
        StartCoroutine(BlinkAnimation());
    }

    private IEnumerator BlinkAnimation() {
        _isBlink = true;

        while (_isBlink) {
            _icon.color = _colorBlink;
            yield return new WaitForSeconds(_timeBlink);
            _icon.color = _colorDefault;
            yield return new WaitForSeconds(_timeBlink);
        }
    }
}
