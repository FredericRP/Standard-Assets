using UnityEditor;
using UnityEngine;

namespace FredericRP.EventManagement
{
  [CustomEditor(typeof(IntGameEvent))]
  public class IntGameEventInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      GUILayout.Label("GameEvent: " + serializedObject.targetObject.name);
      SerializedProperty parameter = serializedObject.FindProperty("parameter");
      if (parameter != null)
        parameter.intValue = EditorGUILayout.IntField("Int Parameter", parameter.intValue);
      else
        EditorGUILayout.HelpBox("Int Game Event should have a parameter property", MessageType.Error);
      if (GUILayout.Button("RAISE"))
      {
        EventHandler.TriggerEvent<int>(serializedObject.targetObject as GameEvent, parameter.intValue);
      }
      if (serializedObject.hasModifiedProperties)
        serializedObject.ApplyModifiedProperties();
    }
  }

}