using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementAlongTheFoundPath : MonoBehaviour
{
    [SerializeField] private int speed;
    private Vector3 nextPosition;
    private Vector3 currentPosition;

    private List<Vector3> listOfAllowedPositions = new List<Vector3>();
    private List<float> listDistancesBettweenCells = new List<float>();
    private List<Vector3> listOfPreviousPositions = new List<Vector3>();

    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private bool canToNextPos = true;

    void Start()
    {
        nextPosition = transform.position;
        currentPosition = transform.position;
    }

    void Update()
    {
        FindingPath();

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }

    //Пошук шляху
    private void FindingPath()
    {
        if (!canToNextPos)
        {
            if (transform.position == nextPosition)
            {
                currentPosition = transform.position;
                canToNextPos = true;
            }
        }

        else if (canToNextPos)
        {
            //Пошук колайдерів із заданою LayerMask
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

            if(collider.Length > 0)
            {
                //Перевірка попередніх позицій
                CheckPreviousPosition(collider);

                //Пошук найближчої позиції
                NearestPosition(collider);

                //Видалення вже непотрібної позиції
                RemoveExtraItemsFromTheList(MaximumStorageOfQuantityPreviousPositions(radius, collider[0]));

                canToNextPos = false;
            }

            else
            {
                Debug.LogError("Поблизу немає шляху із заданим LayerMask. Попробуйте збільшити радіус або розташуйте об'єкт поближче до шляху");
            }
        }
    }


    /// <summary>
    /// Перевірка на наявність попередніх позицій, і створення нового списку з позиціями на які можна перейти
    /// </summary>
    /// <param name="collider"></param>
    private void CheckPreviousPosition(Collider2D[] collider)
    {
        if(listOfPreviousPositions.Count > 0)
        {
            for (int i = 0; i < collider.Length; i++)
            {
                if (!IsPreviousPosition(collider[i], listOfPreviousPositions))
                {
                    listOfAllowedPositions.Add(collider[i].transform.position);
                }
            }
        }

        for (int i = 0; i < listOfPreviousPositions.Count; i++)
        {
            Debug.Log($"previous pos {i} = " + listOfPreviousPositions[i]);
        }
    }

    /// <summary>
    /// При першому запуску передааємо на обрахунок всі колайдери які знайдемо в певному радіусі
    /// Інакше передаємо вже готовий список із дозволених позицій
    /// </summary>
    /// <param name="collider"></param>
    private void NearestPosition(Collider2D[] collider)
    {
        if(listOfAllowedPositions.Count > 0)
        {
            CalculationNearestPosition(listOfAllowedPositions);
        }

        else
        {
            CalculationNearestPosition(GetTransformColliderPositionToVector3(collider));
        }
    }

    /// <summary>
    /// Перетворення масиву з типом Transform у тип Vector3
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    private List<Vector3> GetTransformColliderPositionToVector3(Collider2D[] collider)
    {
        for (int i = 0; i < collider.Length; i++)
        {
            listOfAllowedPositions.Add(collider[i].transform.position);
        }

        return listOfAllowedPositions;
    }

    /// <summary>
    /// Обрахунок найближчої позиції від об'єкта
    /// </summary>
    /// <param name="list"></param>
    private void CalculationNearestPosition(List<Vector3> list)
    {
        float minValue;

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                listDistancesBettweenCells.Add(Mathf.Abs(Vector3.Distance(list[i], currentPosition)));
            }

            minValue = listDistancesBettweenCells.Min();

            for (int i = 0; i < list.Count; i++)
            {
                if (minValue == listDistancesBettweenCells[i])
                {
                    nextPosition = list[i];
                    listOfPreviousPositions.Add(nextPosition);
                }
            }

            listDistancesBettweenCells.Clear();
            list.Clear();
        }
    }

    /// <summary>
    /// Перевіряє чи це позиція на якій об'єкт вже був
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool IsPreviousPosition(Collider2D collider, List<Vector3> position)
    {
        for (int i = 0; i < position.Count; i++)
        {
            if(position[i] == collider.transform.position)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Отримання можливої максимальної кількості клітинок в колі
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="collider"></param>
    /// <returns></returns>
    private float MaximumStorageOfQuantityPreviousPositions(float radius, Collider2D collider)
    {
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
        Debug.Log("amount = " + amount);
        return amount;
    }

    /// <summary>
    /// Видалення зайвих елементів із списку попередніх позицій
    /// Метод зроблений для того щоб масив не був занадто великий, і розрахунок проводився швидше
    /// </summary>
    /// <param name="amount"></param>
    private void RemoveExtraItemsFromTheList(float amount)
    {
        if(listOfPreviousPositions.Count > amount)
        {
            listOfPreviousPositions.RemoveAt(0);
        }
    }
}
