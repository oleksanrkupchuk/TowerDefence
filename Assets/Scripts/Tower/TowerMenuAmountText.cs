using System.Collections;
using UnityEngine;
using TMPro;

public class TowerMenuAmountText : MonoBehaviour
{
    private Vector2 _startPosition;

    [SerializeField]
    private float _stepAxisY;
    [SerializeField]
    private float _distanceForTextMoveAxisY;
    [SerializeField]
    private float _timeLifeObject;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private void Start() {
        StartCoroutine(MoveText());
    }

    public void InitStartPosition(Transform positionTower) {
        _startPosition = new Vector2(positionTower.transform.position.x, positionTower.transform.position.y + _stepAxisY);
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

    public void Destroy() {
        Destroy(gameObject);
    }
}
