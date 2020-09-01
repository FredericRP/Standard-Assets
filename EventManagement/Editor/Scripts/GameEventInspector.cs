using UnityEditor;
using UnityEngine;

namespace FredericRP.EventManagement
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("GameEvent: " + serializedObject.targetObject.name);
            if (GUILayout.Button("RAISE"))
            {
                (serializedObject.targetObject as GameEvent).Raise();
            }
        }
    }

}