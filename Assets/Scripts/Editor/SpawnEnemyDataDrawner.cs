using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnEnemyData))]
public class SpawnEnemyDataDrawner : PropertyDrawer {
    private const float FOLDOUT_HEIGHT = 18;
    private Roads _roads;
    private int _amountFieldInTheClassSpawnEnemyData = 3;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (_roads == null) {
            _roads = GameObject.FindGameObjectWithTag("Roads").GetComponent<Roads>();
        }

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty _roadNameProperty = property.FindPropertyRelative("roadName");
        SerializedProperty _startWaveIconPrroperty = property.FindPropertyRelative("startWaveIcon");
        SerializedProperty _chainDataProperty = property.FindPropertyRelative("spawnEnemies");

        List<string> _roadsList = new List<string>();
        for (int i = 0; i < _roads.GetRoads.Count; i++) {
            _roadsList.Add(_roads.GetRoads[i].name);
        }
        var _availableColors = _roadsList.Select(item => new GUIContent(item)).ToArray();
        var popUpHeight = EditorGUI.GetPropertyHeight(_roadNameProperty);


        var _roadNamePosition = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
        var _startWaveIconPropertyPosition = new Rect(position.x, position.y + FOLDOUT_HEIGHT, position.width, FOLDOUT_HEIGHT);
        var _chainDataPosition = new Rect(position.x, position.y + (FOLDOUT_HEIGHT * 2), position.width, FOLDOUT_HEIGHT);

        _roadNameProperty.intValue = EditorGUI.Popup(_roadNamePosition, new GUIContent(_roadNameProperty.displayName), _roadNameProperty.intValue, _availableColors);
        EditorGUI.PropertyField(_startWaveIconPropertyPosition, _startWaveIconPrroperty, new GUIContent(_startWaveIconPrroperty.displayName));
        EditorGUI.PropertyField(_chainDataPosition, _chainDataProperty, new GUIContent(_chainDataProperty.displayName));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        SerializedProperty _chainDataProperty = property.FindPropertyRelative("spawnEnemies");
        float height = FOLDOUT_HEIGHT * _amountFieldInTheClassSpawnEnemyData;
        if (_chainDataProperty.isExpanded) {
            if (_chainDataProperty.arraySize == 0) {
                height = FOLDOUT_HEIGHT * _amountFieldInTheClassSpawnEnemyData + (3 * FOLDOUT_HEIGHT);
            }
            else {
                for (int i = 0; i < _chainDataProperty.arraySize; i++) {
                    height += EditorGUI.GetPropertyHeight(_chainDataProperty.GetArrayElementAtIndex(i), true);
                }

                height += FOLDOUT_HEIGHT * 2;
            }
        }
        else {
            height = FOLDOUT_HEIGHT * _amountFieldInTheClassSpawnEnemyData + 10;
        }
        return height;
    }
}
