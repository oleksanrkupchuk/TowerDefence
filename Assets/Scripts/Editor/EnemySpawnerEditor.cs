using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private SerializedProperty _roads;
    private SerializedProperty _testChainData;

    private ReorderableList _roadsList;
    private ReorderableList _testChainDataList;

    private EnemySpawner _enemySpawner;
}
