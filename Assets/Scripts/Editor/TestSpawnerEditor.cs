using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TestSpawner))]
public class TestSpawnerEditor : Editor
{
    private SerializedProperty _roads;
    private SerializedProperty _testChainData;

    private ReorderableList _roadsList;
    private ReorderableList _testChainDataList;

    private TestSpawner _testSpawner;

    private void OnEnable() {
        _testSpawner = (TestSpawner)target;

        _roads = serializedObject.FindProperty(nameof(_testSpawner.roads));
        _testChainData = serializedObject.FindProperty(nameof(_testSpawner.testChainData));

        _roadsList = new ReorderableList(serializedObject, _roads) {
            displayAdd = true,
            displayRemove = true,
            draggable = false, // for now disable reorder feature since we later go by index!

            // As the header we simply want to see the usual display name of the CharactersList
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _roads.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) => {
                // get the current element's SerializedProperty
                var element = _roads.GetArrayElementAtIndex(index);

                // Get all characters as string[]
                var availableIDs = _testSpawner.roads;

                var color = GUI.color;
                // Tint the field in red for invalid values
                // either because it is empty or a duplicate
                if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
                    GUI.color = Color.red;
                }
                // Draw the property which automatically will select the correct drawer -> a single line text field
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element);

                // reset to the default color
                GUI.color = color;

                // If the value is invalid draw a HelpBox to explain why it is invalid
                if (string.IsNullOrWhiteSpace(element.stringValue)) {
                    rect.y += EditorGUI.GetPropertyHeight(element);
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "ID may not be empty!", MessageType.Error);
                }
                else if (availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
                    rect.y += EditorGUI.GetPropertyHeight(element);
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Duplicate! ID has to be unique!", MessageType.Error);
                }
            },

            elementHeightCallback = index => {
                var element = _roads.GetArrayElementAtIndex(index);
                var availableIDs = _testSpawner.roads;

                var height = EditorGUI.GetPropertyHeight(element);

                if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
                    height += EditorGUIUtility.singleLineHeight;
                }

                return height;
            },

            onAddCallback = list => {
                // This adds the new element but copies all values of the select or last element in the list
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                newElement.stringValue = "";
            }
        };

        _testChainDataList = new ReorderableList(serializedObject, _testChainData) {
            displayAdd = true,
            displayRemove = true,
            draggable = true, // for the dialogue items we can allow re-ordering

            // As the header we simply want to see the usual display name of the DialogueItems
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _testChainData.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) => {
                // get the current element's SerializedProperty
                var element = _testChainData.GetArrayElementAtIndex(index);

                // Get the nested property fields of the DialogueElement class
                var _road = element.FindPropertyRelative(nameof(TestChainData.road));
                var _wayPointType = element.FindPropertyRelative(nameof(TestChainData.wayPointType));
                var _chainListEnemies = element.FindPropertyRelative(nameof(TestChainData.chainListEnemies));

                var popUpHeight = EditorGUI.GetPropertyHeight(_road);
                // Get the existing character names as GuiContent[]
                //var availableOptions = _testSpawner.roads.Select(item => new GUIContent(item)).ToArray();

                List<string> _roadsList = new List<string>();
                for (int i = 0; i < _testSpawner.testRoadClass.roads.Count; i++) {
                    _roadsList.Add(_testSpawner.testRoadClass.roads[i].name);
                }
                var availableOptions = _roadsList.Select(item => new GUIContent(item)).ToArray();

                // store the original GUI.color
                var color = GUI.color;

                // if the value is invalid tint the next field red
                if (_road.intValue < 0) GUI.color = Color.red;

                // Draw the Popup so you can select from the existing character names
                _road.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight), new GUIContent(_road.displayName), _road.intValue, availableOptions);

                // reset the GUI.color
                GUI.color = color;
                rect.y += popUpHeight;

                //Draw the Character Picture
                var _chainListEnemiesHeight = EditorGUI.GetPropertyHeight(_chainListEnemies);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 5, rect.width, _chainListEnemiesHeight), _chainListEnemies);

                // Draw the Dialogue Text field
                // since we use a PropertyField it will automatically recognize that this field is tagged [TextArea]
                // and will choose the correct drawer accordingly
                var textHeight = EditorGUI.GetPropertyHeight(_wayPointType);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + _chainListEnemiesHeight + 15, rect.width, textHeight), _wayPointType);


            },

            // Get the correct display height of elements in the list
            // according to their values
            // in this case e.g. we add an additional line as a little spacing between elements
            elementHeightCallback = index => {
                var element = _testChainData.GetArrayElementAtIndex(index);

                var _roadName = element.FindPropertyRelative(nameof(TestChainData.road));
                var _wayPointTypeName = element.FindPropertyRelative(nameof(TestChainData.wayPointType));
                var _chainListEnemiesName = element.FindPropertyRelative(nameof(TestChainData.chainListEnemies));

                //height of entire dialog items section
                return EditorGUI.GetPropertyHeight(_roadName) + EditorGUI.GetPropertyHeight(_wayPointTypeName) + EditorGUI.GetPropertyHeight(_chainListEnemiesName) + EditorGUIUtility.singleLineHeight;
            },

            // Overwrite what shall be done when an element is added via the +
            // Reset all values to the defaults for new added elements
            // By default Unity would clone the values from the last or selected element otherwise
            onAddCallback = list => {
                // This adds the new element but copies all values of the select or last element in the list
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var character = newElement.FindPropertyRelative(nameof(TestChainData.road));
                //var text = newElement.FindPropertyRelative(nameof(TestChainData.DialogueText));

                character.intValue = -1;
                //text.stringValue = "";
            }
        };
    }

    public override void OnInspectorGUI() {
        DrawScriptField();

        // load real target values into SerializedProperties
        serializedObject.Update();

        _roadsList.DoLayoutList();

        _testChainDataList.DoLayoutList();

        _testSpawner.testRoadClass = (TestRoad)EditorGUILayout.ObjectField("Test Road", _testSpawner.testRoadClass, typeof(TestRoad), true);

        // Write back changed values into the real target
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField() {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Test Spawner", MonoScript.FromMonoBehaviour((TestSpawner)target), typeof(TestSpawner), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}
