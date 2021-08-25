using FredericRP.Popups;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PopupDescriptor))]
public class PopupDescriptorEditor : Editor
{
  SerializedProperty popupSource;
  SerializedProperty poolObjectName;
  SerializedProperty prefab;

  private void OnEnable()
  {
    popupSource = serializedObject.FindProperty("popupSource");
    poolObjectName = serializedObject.FindProperty("pooledObjectName");
    prefab = serializedObject.FindProperty("prefab");
  }
  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    EditorGUILayout.BeginHorizontal();
    if (popupSource.enumValueIndex == 0)
      EditorGUILayout.PropertyField(poolObjectName);
    else
      EditorGUILayout.PropertyField(prefab);
    int selectionFromInspector = popupSource.intValue;
    string[] enumNamesList = System.Enum.GetNames(typeof(PopupDescriptor.PopupSource));
    int actualSelected = EditorGUILayout.Popup("", selectionFromInspector, enumNamesList, GUILayout.Width(100));
    popupSource.intValue = actualSelected;

    EditorGUILayout.EndHorizontal();

    serializedObject.ApplyModifiedProperties();
  }
}
