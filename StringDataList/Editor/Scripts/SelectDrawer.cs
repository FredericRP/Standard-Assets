using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FredericRP.StringDataList
{
  [CustomPropertyDrawer(typeof(SelectAttribute))]
  public class SelectDrawer : PropertyDrawer
  {
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // Using BeginProperty / EndProperty on the parent property means that
      // prefab override logic works on the entire property.
      EditorGUI.BeginProperty(position, label, property);

      // Draw label
      Rect firstLineRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

      // - line 1 : selected value
      Rect valueRect = new Rect(firstLineRect.x, position.y, firstLineRect.width - 32, position.height);
      Rect buttonRect = new Rect(firstLineRect.x + firstLineRect.width - 32, position.y, 32, position.height);

      // NEW : list can be a list of list names
      string[] identifierList = (attribute as SelectAttribute).identifier.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      List<string> list = new List<string>();
      for (int i = 0; i < identifierList.Length; i++)
      {
        list.AddRange(DataListLoader.GetDataList(identifierList[i].Trim()));
      }
      // 2. result list
      if (list.Count > 0)
      {
        int selectedIndex = -1;
        selectedIndex = list.FindIndex(element => element.Equals(property.stringValue, StringComparison.InvariantCulture));
        int previousIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        EditorGUI.BeginChangeCheck();

        int newSelectedIndex = EditorGUI.Popup(valueRect, selectedIndex, list.ToArray());
        if (EditorGUI.EndChangeCheck())
          property.stringValue = list[newSelectedIndex];
        Color previousGuiColor = GUI.color;
        GUI.color = Color.red;
        if (GUI.Button(buttonRect, "x", EditorStyles.miniButton))
        {
          property.stringValue = null;
        }
        GUI.color = previousGuiColor;

        EditorGUI.indentLevel = previousIndentLevel;
      } else
      {
        property.stringValue = EditorGUI.TextField(valueRect, property.stringValue);
      }
      property.serializedObject.ApplyModifiedProperties();
      // */

      EditorGUI.EndProperty();
    }

  }
}