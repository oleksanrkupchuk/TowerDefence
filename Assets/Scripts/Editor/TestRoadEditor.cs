using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TestRoad))]
public class TestRoadEditor : Editor
{
    private SerializedProperty _roads;

    private ReorderableList _testRoadList;

    private TestRoad _testRoad;

    private void OnEnable() {
        _testRoad = (TestRoad)target;

        _roads = serializedObject.FindProperty(nameof(_testRoad.roads));

        _testRoadList = new ReorderableList(serializedObject, _roads) {
            displayAdd = true,
            displayRemove = true,
            draggable = false, // for now disable reorder feature since we later go by index!

            // As the header we simply want to see the usual display name of the CharactersList
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _roads.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) => {
                // get the current element's SerializedProperty
                var element = _roads.GetArrayElementAtIndex(index);

                var _name = element.FindPropertyRelative(nameof(RoadData.name));
                var _road = element.FindPropertyRelative(nameof(RoadData.road));

                var _chainListEnemiesHeight = EditorGUI.GetPropertyHeight(_name);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, _chainListEnemiesHeight), _name);

                // Draw the Dialogue Text field
                // since we use a PropertyField it will automatically recognize that this field is tagged [TextArea]
                // and will choose the correct drawer accordingly
                var textHeight = EditorGUI.GetPropertyHeight(_road);
                EditorGUI.PropertyField(new Rect(rect.x + 10, rect.y + _chainListEnemiesHeight, rect.width - 10, textHeight), _road);
            },

            elementHeightCallback = index => {
                var element = _roads.GetArrayElementAtIndex(index);
                var availableIDs = _testRoad.roads;

                var height = EditorGUI.GetPropertyHeight(element);

                return height;
            },
        };
    }

    public override void OnInspectorGUI() {
        DrawScriptField();

        // load real target values into SerializedProperties
        serializedObject.Update();

        _testRoadList.DoLayoutList();

        // Write back changed values into the real target
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField() {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((TestRoad)target), typeof(TestRoad), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}
