using UnityEditor;
using UnityEngine;

namespace FredericRP.EventManagement
{
  [CustomEditor(typeof(FloatGameEvent))]
  public class FloatGameEventInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      GUILayout.Label("GameEvent: " + serializedObject.targetObject.name);
      SerializedProperty parameter = serializedObject.FindProperty("parameter");
      if (parameter != null)
        parameter.floatValue = EditorGUILayout.FloatField("Float Parameter", parameter.floatValue);
      else
        EditorGUILayout.HelpBox("Float Game Event should have a parameter property", MessageType.Error);
      if (GUILayout.Button("RAISE"))
      {
        EventHandler.TriggerEvent<float>(serializedObject.targetObject as GameEvent, parameter.floatValue);
      }
      if (serializedObject.hasModifiedProperties)
        serializedObject.ApplyModifiedProperties();
    }
  }

}