using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenuAmountText : MonoBehaviour {
    private Vector2 _startPosition;

    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _timeLifeObject;
    [SerializeField]
    private TextMeshProUGUI _amountText;
    [SerializeField]
    private Image _coinIcon;
    [SerializeField]
    private Sprite _spriteCoin;

    public void Init() {
        _startPosition = new Vector2(transform.position.x, transform.position.y);

        _coinIcon.sprite = _spriteCoin;
        _coinIcon.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update() {
        if (gameObject.activeSelf == true) {
            if (transform.position.y >= _target.position.y) {
                DisableObject();
            }
        }
    }

    public void SetStartPosition() {
        transform.position = _startPosition;
    }

    public void MoveText() {
        //LeanTween.moveLocalY(gameObject, _target.position.y, _timeLifeObject);
        LeanTween.moveY(gameObject, _target.position.y, _timeLifeObject);
    }

    public void StopMove() {
        LeanTween.cancel(gameObject);
    }

    private void DisableObject() {
        if (_coinIcon.gameObject.activeSelf == true) {
            _coinIcon.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void SetTextAmount(float amount, bool enableCoinIcon) {
        if (enableCoinIcon) {
            _coinIcon.gameObject.SetActive(true);
        }
        _amountText.text = "+ " + amount;
    }
}
