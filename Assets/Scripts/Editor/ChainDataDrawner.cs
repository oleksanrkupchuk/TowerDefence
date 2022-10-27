//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//public class ChainDataDrawner : PropertyDrawer {
//    private ReorderableList _roadsList;

//    _roadsList = new ReorderableList(serializedObject, _roads) {
//        displayAdd = true,
//            displayRemove = true,
//            draggable = false, // for now disable reorder feature since we later go by index!

//            // As the header we simply want to see the usual display name of the CharactersList
//            drawHeaderCallback = rect => EditorGUI.LabelField(rect, _roads.displayName),

//            // How shall elements be displayed
//            drawElementCallback = (rect, index, focused, active) => {
//                // get the current element's SerializedProperty
//                var element = _roads.GetArrayElementAtIndex(index);

//                // Get all characters as string[]
//                var availableIDs = _testSpawner.roads;

//                var color = GUI.color;
//                // Tint the field in red for invalid values
//                // either because it is empty or a duplicate
//                if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
//                    GUI.color = Color.red;
//                }
//                // Draw the property which automatically will select the correct drawer -> a single line text field
//                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element);

//                // reset to the default color
//                GUI.color = color;

//                // If the value is invalid draw a HelpBox to explain why it is invalid
//                if (string.IsNullOrWhiteSpace(element.stringValue)) {
//                    rect.y += EditorGUI.GetPropertyHeight(element);
//                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "ID may not be empty!", MessageType.Error);
//                }
//                else if (availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
//                    rect.y += EditorGUI.GetPropertyHeight(element);
//                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Duplicate! ID has to be unique!", MessageType.Error);
//                }
//            },

//            elementHeightCallback = index => {
//                var element = _roads.GetArrayElementAtIndex(index);
//                var availableIDs = _testSpawner.roads;

//                var height = EditorGUI.GetPropertyHeight(element);

//                if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1) {
//                    height += EditorGUIUtility.singleLineHeight;
//                }

//                return height;
//            },

//            onAddCallback = list => {
//                // This adds the new element but copies all values of the select or last element in the list
//                list.serializedProperty.arraySize++;

//                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
//                newElement.stringValue = "";
//            }
//        };
//}
