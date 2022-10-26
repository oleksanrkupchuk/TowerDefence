using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadType {
    First,
    Second,
    Third
}

public enum RoadPart {
    Top,
    Center,
    Bottom
}

public class Road : MonoBehaviour
{
    [SerializeField]
    private List<RoadData> _roads = new List<RoadData>();
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
