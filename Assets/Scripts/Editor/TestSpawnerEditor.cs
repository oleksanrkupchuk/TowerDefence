using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TestSpawner))]
public class TestSpawnerEditor : Editor {
    private SerializedProperty _roadsProperty;
    private SerializedProperty _testChainProperty;
    private SerializedProperty _testAppleProperty;

    private ReorderableList _roadsList;
    private ReorderableList _testChainDataList;
    private ReorderableList _appleList;

    private TestSpawner _testSpawner;

    private void OnEnable() {
        _testSpawner = (TestSpawner)target;

        _roadsProperty = serializedObject.FindProperty(nameof(_testSpawner.roads));
        _testChainProperty = serializedObject.FindProperty(nameof(_testSpawner.testChainData));

        _roadsList = new ReorderableList(serializedObject, _roadsProperty) {
            displayAdd = true,
            displayRemove = true,
            draggable = false, // for now disable reorder feature since we later go by index!

            // As the header we simply want to see the usual display name of the CharactersList
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _roadsProperty.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) => {
                // get the current element's SerializedProperty
                var element = _roadsProperty.GetArrayElementAtIndex(index);

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
                var element = _roadsProperty.GetArrayElementAtIndex(index);
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

        _testChainDataList = new ReorderableList(serializedObject, _testChainProperty) {
            displayAdd = true,
            displayRemove = true,
            draggable = true, // for the dialogue items we can allow re-ordering

            // As the header we simply want to see the usual display name of the DialogueItems
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _testChainProperty.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) => {
                // get the current element's SerializedProperty
                var element = _testChainProperty.GetArrayElementAtIndex(index);

                // Get the nested property fields of the DialogueElement class
                var _roadProperty = element.FindPropertyRelative(nameof(TestChainData.road));
                var _wayPointType = element.FindPropertyRelative(nameof(TestChainData.wayPointType));
                var _chainListEnemies = element.FindPropertyRelative(nameof(TestChainData.chainListEnemies));
                _testAppleProperty = element.FindPropertyRelative(nameof(TestChainData.testApples));

                AppleList();

                var popUpHeight = EditorGUI.GetPropertyHeight(_roadProperty);
                // Get the existing character names as GuiContent[]
                //var availableOptions = _testSpawner.roads.Select(item => new GUIContent(item)).ToArray();
                var availableOptions = new GUIContent[100];

                if (_testSpawner.testRoadClass != null) {

                    List<string> _roadsList = new List<string>();
                    for (int i = 0; i < _testSpawner.testRoadClass.roads.Count; i++) {
                        _roadsList.Add(_testSpawner.testRoadClass.roads[i].name);
                    }

                    availableOptions = _roadsList.Select(item => new GUIContent(item)).ToArray();
                }
                else {
                    Debug.LogWarning("field testRoadClass is null");
                }

                // store the original GUI.color
                var color = GUI.color;
                // if the value is invalid tint the next field red
                if (_roadProperty.intValue < 0) GUI.color = Color.red;
                // Draw the Popup so you can select from the existing character names
                _roadProperty.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight), new GUIContent(_roadProperty.displayName), _roadProperty.intValue, availableOptions);
                // reset the GUI.color
                GUI.color = color;
                rect.y += popUpHeight;

                // Draw the Dialogue Text field
                var _chainListEnemiesHeight = EditorGUI.GetPropertyHeight(_chainListEnemies);
                var wayPointTypeHeight = EditorGUI.GetPropertyHeight(_wayPointType);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + _chainListEnemiesHeight + 15, rect.width, wayPointTypeHeight), _wayPointType);

                //Draw the Character Picture
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 5, rect.width, _chainListEnemiesHeight), _chainListEnemies);

                var tesAppleHeight = EditorGUI.GetPropertyHeight(_testAppleProperty);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + 15 + _chainListEnemiesHeight + wayPointTypeHeight, rect.width, tesAppleHeight), _testAppleProperty);
            },

            // Get the correct display height of elements in the list
            // according to their values
            // in this case e.g. we add an additional line as a little spacing between elements
            elementHeightCallback = index => {
                var element = _testChainProperty.GetArrayElementAtIndex(index);

                var _roadName = element.FindPropertyRelative(nameof(TestChainData.road));
                var _wayPointTypeName = element.FindPropertyRelative(nameof(TestChainData.wayPointType));
                var _chainListEnemiesName = element.FindPropertyRelative(nameof(TestChainData.chainListEnemies));
                var _testTwo = element.FindPropertyRelative(nameof(TestChainData.testApples));

                //height of entire dialog items section
                return EditorGUI.GetPropertyHeight(_roadName) + EditorGUI.GetPropertyHeight(_wayPointTypeName) + 
                EditorGUI.GetPropertyHeight(_chainListEnemiesName) + EditorGUI.GetPropertyHeight(_testTwo) + EditorGUIUtility.singleLineHeight;
            },

            // Overwrite what shall be done when an element is added via the +
            // Reset all values to the defaults for new added elements
            // By default Unity would clone the values from the last or selected element otherwise
            onAddCallback = list => {
                // This adds the new element but copies all values of the select or last element in the list
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var road = newElement.FindPropertyRelative(nameof(TestChainData.road));
                //var text = newElement.FindPropertyRelative(nameof(TestChainData.DialogueText));

                road.intValue = -1;
                //text.stringValue = "";
            }
        };
    }

    public void AppleList() {
        _appleList = new ReorderableList(serializedObject, _testAppleProperty) {
            displayAdd = true,
            displayRemove = true,
            draggable = true, // for now disable reorder feature since we later go by index!

            // As the header we simply want to see the usual display name of the CharactersList
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _testAppleProperty.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) => {
                var element = _testAppleProperty.GetArrayElementAtIndex(index);

                var _appleProperty = element.FindPropertyRelative(nameof(TestApple.apple));

                var popUpHeight = EditorGUI.GetPropertyHeight(_appleProperty);
                var availableOptions = new GUIContent[100];

                List<string> _avaibleRoadsList = new List<string>();

                if (_testSpawner.testRoadClass != null) {

                    for (int i = 0; i < _testSpawner.testRoadClass.roads.Count; i++) {
                        _avaibleRoadsList.Add(_testSpawner.testRoadClass.roads[i].name);
                    }

                    availableOptions = _avaibleRoadsList.Select(item => new GUIContent(item)).ToArray();
                }
                else {
                    Debug.LogWarning("field testRoadClass is null");
                }

                var color = GUI.color;
                if (_appleProperty.intValue < 0) GUI.color = Color.red;
                _appleProperty.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight), new GUIContent(_appleProperty.displayName), _appleProperty.intValue, availableOptions);
                GUI.color = color;
                rect.y += popUpHeight;
            },

            elementHeightCallback = index => {
                var element = _testAppleProperty.GetArrayElementAtIndex(index);
                var _appleProperty = element.FindPropertyRelative(nameof(TestApple.apple));
                return EditorGUI.GetPropertyHeight(_appleProperty) + EditorGUIUtility.singleLineHeight;
            },
        };
    }

    public override void OnInspectorGUI() {
        DrawScriptField();

        // load real target values into SerializedProperties
        serializedObject.Update();

        _testSpawner.testRoadClass = (TestRoad)EditorGUILayout.ObjectField("Test Road", _testSpawner.testRoadClass, typeof(TestRoad), true);

        _roadsList.DoLayoutList();

        _testChainDataList.DoLayoutList();

        _appleList.DoLayoutList();

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
