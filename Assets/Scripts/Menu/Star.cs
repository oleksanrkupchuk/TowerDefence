using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour {
    private float _timer;
    private float _time;

    [SerializeField]
    private Image _starCenter;
    [SerializeField]
    private float _timeAnimation;
    [SerializeField]
    private float _timeDelayForShowAnimation;
    [SerializeField]
    private float _timePlayBlinkAnimation;
    [SerializeField]
    private float _timeBlink;
    [SerializeField]
    private bool _isBlink;
    [SerializeField]
    private Color _colorBlink;
    [SerializeField]
    private Color _colorDefault;

    public bool endAnamation;

    void Start() {
        _starCenter.fillAmount = 0;
        _timer = _timeAnimation;
    }

    private void Update() {
        if (_isBlink) {
            _timePlayBlinkAnimation -= Time.deltaTime;
            print("time = " + _timePlayBlinkAnimation);
            if (_timePlayBlinkAnimation < 0) {
                _isBlink = false;
            }
        }
    }

    public void StartAnimation() {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation() {
        yield return new WaitForSeconds(_timeDelayForShowAnimation);

        while (_timer >= 0) {
            //print("play");
            //_time += Time.deltaTime;
            _timer -= Time.deltaTime;
            //_timer -= _time * _time;

            float _amount = Mathf.Abs(_timer - _timeAnimation) / _timeAnimation;
            _starCenter.fillAmount = _amount;

            yield return null;
        }

        _isBlink = true;

        StartCoroutine(BlinkAnimation());
    }

    private IEnumerator BlinkAnimation() {
        while (_isBlink) {
            //print("blink");
            _starCenter.color = _colorBlink;
            yield return new WaitForSeconds(_timeBlink);
            _starCenter.color = _colorDefault;
            yield return new WaitForSeconds(_timeBlink);
        }

        endAnamation = true;
    }
}
