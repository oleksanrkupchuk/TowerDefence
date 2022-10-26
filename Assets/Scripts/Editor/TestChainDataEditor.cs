using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestChainData))]
public class TestChainDataEditor : Editor
{
    private TestChainData _testChainData;

    private void OnEnable() {
        //_testChainData = (TestChainData)target;
    }
}
