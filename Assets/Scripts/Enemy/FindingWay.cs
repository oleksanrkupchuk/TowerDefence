using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindingWay : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private bool _canToNextPos = true;
    public bool stopMovement = false;

    private Vector3 _nextPosition;
    public Vector3 NextPosition { get => _nextPosition; }
    private Vector3 _currentPosition;

    private List<Vector3> _listOfAllowedPositions = new List<Vector3>();
    private List<float> _listDistancesBettweenCells = new List<float>();
    private List<Vector3> _listOfPreviousPositions = new List<Vector3>();

    private void Awake() {
        _nextPosition = transform.position;
        _currentPosition = transform.position;
    }

    public void FindingPath() {
        if (!stopMovement) {
            if (!_canToNextPos) {
                if (transform.position == _nextPosition) {
                    _currentPosition = transform.position;
                    _canToNextPos = true;
                }
            }

            else if (_canToNextPos) {
                //Пошук колайдерів із заданою LayerMask
                Collider2D[] getNearestColliders = Physics2D.OverlapCircleAll(transform.position, _radius, _layerMask);

                if (getNearestColliders.Length > 0) {
                    //Перевірка попередніх позицій
                    CheckPreviousPosition(getNearestColliders);

                    //Пошук найближчої позиції
                    NearestPosition(getNearestColliders);

                    //Видалення вже непотрібної позиції
                    RemoveExtraPositionFromTheList(MaximumStorageOfQuantityPreviousPositions(_radius, getNearestColliders[0]));

                    _canToNextPos = false;
                }

                else {
                    Debug.LogWarning("There is no path with the specified LayerMask nearby. Try increasing the radius or placing the object closer to the path");
                }
            }
        }
        else {
            return;
        }
    }

    private void CheckPreviousPosition(Collider2D[] collider) {
        if (_listOfPreviousPositions.Count > 0) {
            for (int i = 0; i < collider.Length; i++) {
                if (!CheckPreviousPosition(collider[i], _listOfPreviousPositions)) {
                    _listOfAllowedPositions.Add(collider[i].transform.position);
                }
            }
        }
    }

    /// <summary>
    /// Перевіряє чи це позиція на якій об'єкт вже був
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CheckPreviousPosition(Collider2D collider, List<Vector3> position) {
        for (int i = 0; i < position.Count; i++) {
            if (position[i] == collider.transform.position) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// При першому запуску передааємо на обрахунок всі колайдери які знайдемо в певному радіусі
    /// Інакше передаємо вже готовий список із дозволених позицій
    /// </summary>
    /// <param name="collider"></param>
    private void NearestPosition(Collider2D[] collider) {
        if (_listOfAllowedPositions.Count > 0) {
            CalculationNearestPosition(_listOfAllowedPositions);
        }

        else {
            CalculationNearestPosition(GetTransformColliderPositionToVector3(collider));
        }
    }

    /// <summary>
    /// Обрахунок найближчої позиції від об'єкта
    /// </summary>
    /// <param name="list"></param>
    private void CalculationNearestPosition(List<Vector3> list) {
        float _minValue;

        if (list.Count > 0) {
            for (int i = 0; i < list.Count; i++) {
                _listDistancesBettweenCells.Add(Mathf.Abs(Vector3.Distance(list[i], _currentPosition)));
            }

            _minValue = _listDistancesBettweenCells.Min();

            for (int i = 0; i < list.Count; i++) {
                if (_minValue == _listDistancesBettweenCells[i]) {
                    _nextPosition = list[i];
                    //Debug.Log("next pos = " + nextPosition);
                    _listOfPreviousPositions.Add(_nextPosition);
                }
            }

            _listDistancesBettweenCells.Clear();
            list.Clear();
        }
    }

    /// <summary>
    /// Перетворення масиву з типом Transform у тип Vector3
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    private List<Vector3> GetTransformColliderPositionToVector3(Collider2D[] collider) {
        for (int i = 0; i < collider.Length; i++) {
            _listOfAllowedPositions.Add(collider[i].transform.position);
        }

        return _listOfAllowedPositions;
    }

    /// <summary>
    /// Getting the maximum possible number of cells in a circle, depends on the radius of the circle
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="collider"></param>
    /// <returns></returns>
    private float MaximumStorageOfQuantityPreviousPositions(float radius, Collider2D collider) {
        float colliderHeight = collider.GetComponent<BoxCollider2D>().size.x;
        float colliderWidth = collider.GetComponent<BoxCollider2D>().size.y;

        float colliderScaleX = collider.GetComponent<Transform>().localScale.x;
        float colliderScaleY = collider.GetComponent<Transform>().localScale.y;

        float amountX;
        float amountY;
        float amount;

        amountX = (radius * 2) / (colliderHeight * colliderScaleX);
        amountY = (radius * 2) / (colliderWidth * colliderScaleY);
        amount = amountX * amountY;

        return amount;
    }

    /// <summary>
    /// Видалення зайвих позицій із списку попередніх позицій
    /// Метод зроблений для того щоб масив не був занадто великий, і розрахунок проводився швидше
    /// </summary>
    /// <param name="amount"></param>
    private void RemoveExtraPositionFromTheList(float amount) {
        if (_listOfPreviousPositions.Count > amount) {
            _listOfPreviousPositions.RemoveAt(0);
        }
    }
}
