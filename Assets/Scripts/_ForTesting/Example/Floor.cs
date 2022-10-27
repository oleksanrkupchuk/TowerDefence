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
}
