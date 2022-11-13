using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CustomEditor(typeof(House))]
public class HouseEditor : Editor
{
    private SerializedProperty _sizeTablesProperty;
    private SerializedProperty _floorsProperty;
    private SerializedProperty _roomsProperty;

    private ReorderableList _sizeFloorsList;
    private ReorderableList _floorsList;
    private ReorderableList _roomColorList;

    private House _house;

    private void OnEnable() {
        _house = (House)target;

        _sizeTablesProperty = serializedObject.FindProperty(nameof(_house.sizeFloors));
        _floorsProperty = serializedObject.FindProperty(nameof(_house.floors));

        InitSizeFloorsList();
        InitFloorsList();
    }

    private void InitSizeFloorsList() {
        _sizeFloorsList = new ReorderableList(serializedObject, _sizeTablesProperty) {
            displayAdd = true,
            displayRemove = true,
            draggable = false,

            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _sizeTablesProperty.displayName),

            drawElementCallback = (rect, index, focused, active) => {
                var element = _sizeTablesProperty.GetArrayElementAtIndex(index);

                var avaibleSizeTable = _house.sizeFloors;

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element);
            },

            elementHeightCallback = index => {
                var element = _sizeTablesProperty.GetArrayElementAtIndex(index);
                var avaibleSizeTable = _house.sizeFloors;

                var height = EditorGUI.GetPropertyHeight(element);

                if (string.IsNullOrWhiteSpace(element.stringValue) || avaibleSizeTable.Count(item => string.Equals(item, element.stringValue)) > 1) {
                    height += EditorGUIUtility.singleLineHeight;
                }

                return height;
            },
        };
    }

    private void InitFloorsList() {
        _floorsList = new ReorderableList(serializedObject, _floorsProperty) {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _floorsProperty.displayName),

            drawElementCallback = (rect, index, focused, active) => {
                var element = _floorsProperty.GetArrayElementAtIndex(index);

                var _sizeProperty = element.FindPropertyRelative(nameof(Floor.size));
                _roomsProperty = element.FindPropertyRelative(nameof(Floor.rooms));

                var popUpHeight = EditorGUI.GetPropertyHeight(_floorsProperty);

                var availableSizeRoom = _house.sizeFloors.Select(item => new GUIContent(item)).ToArray();
                _sizeProperty.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight), new GUIContent(_sizeProperty.displayName), _sizeProperty.intValue, availableSizeRoom);
                rect.y += popUpHeight;

                var _roomsFieldHeight = EditorGUI.GetPropertyHeight(_roomsProperty);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 10, rect.width, 
                    _roomsFieldHeight), _roomsProperty);
            },

           
            elementHeightCallback = index => {
                var element = _floorsProperty.GetArrayElementAtIndex(index);

                var _sizeProperty = element.FindPropertyRelative(nameof(Floor.size));
                var _roomsProperty = element.FindPropertyRelative(nameof(Floor.rooms));

                return EditorGUI.GetPropertyHeight(_sizeProperty) + EditorGUI.GetPropertyHeight(_roomsProperty) + EditorGUIUtility.singleLineHeight;
            },
        };
    }

    public override void OnInspectorGUI() {
        DrawScriptField();

        serializedObject.Update();

        _house.roomColors = (RoomColors)EditorGUILayout.ObjectField("Room Colors", _house.roomColors, typeof(RoomColors), true);

        _sizeFloorsList.DoLayoutList();
        _floorsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField() {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("House", MonoScript.FromMonoBehaviour((House)target), typeof(House), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}
