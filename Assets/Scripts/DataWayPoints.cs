using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataWayPoints
{
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    public List<Transform> WayPoints { get => _wayPoints; }
}
