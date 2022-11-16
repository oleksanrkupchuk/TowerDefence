using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadPart {
    Top,
    Center,
    Bottom
}

public class Roads : MonoBehaviour
{
    [SerializeField]
    private List<RoadData> _roads = new List<RoadData>();
    public List<RoadData> GetRoads { get => _roads; }

    public List<Transform> GetPartRoad(int roadName, RoadPart roadPart) {
        RoadData _roadData = GetRoad(roadName);
        return _roadData.road[(int)roadPart].wayPoints;
    }

    private RoadData GetRoad(int roadName) {
        for (int numberRoad = 0; numberRoad < _roads.Count; numberRoad++) {
            if(roadName == numberRoad) {
                return _roads[numberRoad];
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
