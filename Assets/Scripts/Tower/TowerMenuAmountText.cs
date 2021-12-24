using System.Collections;
using UnityEngine;
using TMPro;

public class TowerMenuAmountText : MonoBehaviour
{
    private Tower _tower;
    private Vector2 _startPosition;

    [SerializeField]
    private float _stepAxisY;
    [SerializeField]
    private float _distanceForTextMoveAxisY;
    [SerializeField]
    private float _timeLifeObject;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    public void InitTower(Tower tower) {
        _tower = tower;
    }

    private void Start() {
        InitStartPosition();
        StartCoroutine(MoveText());
    }

    private void InitStartPosition() {
        _startPosition = new Vector2(_tower.transform.position.x, _tower.transform.position.y + _stepAxisY);
        transform.position = _startPosition;
    }

    private IEnumerator MoveText() {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + _distanceForTextMoveAxisY, _timeLifeObject);
        yield return new WaitForSeconds(_timeLifeObject);
        DestroyText();
    }

    private void DestroyText() {
        Destroy(gameObject);
    }

    public void SetTextAmount(float amount) {
        _amountText.text = "" + amount;
    }
}
