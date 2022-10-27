using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestChainData
{
    public int road;
    public RoadPart wayPointType = new RoadPart();
    public List<ChainOfEnemies> chainListEnemies = new List<ChainOfEnemies>();
    public List<TestApple> testApples = new List<TestApple>();
}

[Serializable]
public class TestApple{
    public int apple;
}
