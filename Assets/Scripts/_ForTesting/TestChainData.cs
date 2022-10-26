using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TestChainData", menuName = "ScriptableObjects/TestChainData", order = 5)]
[Serializable]
public class TestChainData
{
    public int road;
    public RoadPart wayPointType = new RoadPart();
    public List<ChainOfEnemies> chainListEnemies = new List<ChainOfEnemies>();
}
