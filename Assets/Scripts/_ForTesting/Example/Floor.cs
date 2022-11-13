using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Floor 
{
    public int size;
    public List<Room> rooms = new List<Room>();
}

[Serializable]
public class Room {
    public int color;
    public int amountOfTables;
    public int amountOfChairs;
    public bool isCollapse;
    public List<SubRoom> subRooms = new List<SubRoom>();
}

[Serializable]
public class SubRoom {
    public List<string> size = new List<string>();
}
