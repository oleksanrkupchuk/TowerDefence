using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadPart {
    Top,
    Center,
    Bottom
}

public class Road : MonoBehaviour
{
    [SerializeField]
    private List<RoadData> _roads = new List<RoadData>();

    public List<Transform> GetPartRoad(string roadName, RoadPart roadPart) {
        RoadData _roadData = GetRoad(roadName);
        return _roadData.road[(int)roadPart].wayPoints;
    }

    private RoadData GetRoad(string roadName) {
        foreach (var road in _roads) {
            if (road.name == roadName) {
                return road;
            }
        }

        Debug.LogWarning($"Road '{roadName}' does not exist");
        return null;
    }
}

[Serializable]
public class RoadData {
    public string name;
    public List<WayPointData> road = new List<WayPointData>();
}

[Serializable]
public class WayPointData {
    public List<Transform> wayPoints = new List<Transform>();
}
