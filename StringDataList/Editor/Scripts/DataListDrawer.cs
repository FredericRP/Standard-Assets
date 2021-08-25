using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FredericRP.StringDataList
{
  [CustomPropertyDrawer(typeof(DataListAttribute))]
  public class DataListDrawer : PropertyDrawer
  {
    List<string> dataList;

    DataListAttribute dataListAttribute
    {
      get { return (DataListAttribute)attribute; }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      dataList = DataListLoader.GetDataList(dataListAttribute.identifier);
      //
      Rect headerPosition = position;
      headerPosition.height = 16f;
      SerializedProperty list = property.FindPropertyRelative("list");
      // Draw array header
      if (list == null || list.arraySize == 0)
      {
        Color previousGUIColor = GUI.color;
        Rect buttonPosition = EditorGUI.PrefixLabel(headerPosition, label);
        buttonPosition.width = 32;
        GUI.color = Color.green;
        if (GUI.Button(buttonPosition, duplicateButtonContent, EditorStyles.miniButton))
        {
          if (list != null)
          {
            list.InsertArrayElementAtIndex(0);
            // Auto expand on first add
            list.isExpanded = true;
          }
        }
        GUI.color = previousGUIColor;
      }
      else
      {
        list.isExpanded = EditorGUI.Foldout(headerPosition, list.isExpanded, label.text + "\t (" + list.arraySize + ")", true);
        // Go on to next line
        headerPosition.y += 18f;
        ShowList(headerPosition, list);
      }
    }

    private static GUIContent
        moveDownButtonContent = new GUIContent("\u25bc", "move down"),
        moveUpButtonContent = new GUIContent("\u25b2", "move up"),
        duplicateButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete");
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    public void ShowList(Rect position, SerializedProperty list, bool showListSize = false)
    {
      EditorGUI.indentLevel += 1;
      if (list.isExpanded)
      {
        if (showListSize)
        {
          EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
        }
        for (int i = 0; i < list.arraySize; i++)
        {
          Rect propertyPosition = position;
          Rect buttonPosition = propertyPosition;
          buttonPosition.width = 96;
          propertyPosition.width -= 96;
          buttonPosition.x = propertyPosition.width + propertyPosition.x;
          // Use season name if any
          GUIContent dataName = new GUIContent((dataList != null && i < dataList.Count) ? ("[" + i + "] " + dataList[i]) : (dataListAttribute.identifier + " " + (i + 1) + ""));
          SerializedProperty item = list.GetArrayElementAtIndex(i);
          EditorGUI.PropertyField(propertyPosition, item, dataName, true);
          // Buttons to order list
          if (!ShowButtons(buttonPosition, list, i))
            return;
          position.y += EditorGUI.GetPropertyHeight(item, true);// * (item.isExpanded ? item.CountInProperty() : 1); ;
        }
      }
      EditorGUI.indentLevel -= 1;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      /* We support this drawer only on AssetData.SeasonIntList
      if (property.type != typeof(AssetData.SeasonIntList).Name)
      {
          return base.GetPropertyHeight(property, label);
      }// */
      SerializedProperty list = property.FindPropertyRelative("list");
      if (list != null)
      {
        if (list.isExpanded)
        {
          if (list.arraySize <= 0)
            return EditorGUIUtility.singleLineHeight;
          else
          {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            // We must check each item of the list to see if it's expanded or not
            for (int i = 0; i < list.arraySize; i++)
            {
              SerializedProperty item = list.GetArrayElementAtIndex(i);
              height += EditorGUI.GetPropertyHeight(item, true);// * (item.isExpanded ? item.CountInProperty() : 1);
            }
            return height;
          }
          //return (list.arraySize - 1) * (2 + base.GetPropertyHeight(property, label)) + 34f;
        }
        else
          return EditorGUIUtility.singleLineHeight;
      }
      return EditorGUI.GetPropertyHeight(property, label);
    }

    private static bool ShowButtons(Rect position, SerializedProperty list, int index)
    {
      Rect buttonPosition = position;
      buttonPosition.width /= 4;
      Color previousGUIColor = GUI.color;
      bool wasEnabled = GUI.enabled;
      GUI.enabled = (index > 0) && wasEnabled;
      if (GUI.Button(buttonPosition, moveUpButtonContent, EditorStyles.miniButtonLeft))
      {
        list.MoveArrayElement(index, index - 1);
      }
      buttonPosition.x += buttonPosition.width;
      GUI.enabled = (index < list.arraySize - 1) && wasEnabled;
      if (GUI.Button(buttonPosition, moveDownButtonContent, EditorStyles.miniButtonMid))
      {
        list.MoveArrayElement(index, index + 1);
      }
      buttonPosition.x += buttonPosition.width;
      GUI.enabled = wasEnabled;
      GUI.color = Color.green;
      if (GUI.Button(buttonPosition, duplicateButtonContent, EditorStyles.miniButtonMid))
      {
        list.InsertArrayElementAtIndex(index);
      }
      buttonPosition.x += buttonPosition.width;
      GUI.color = Color.red;
      if (GUI.Button(buttonPosition, deleteButtonContent, EditorStyles.miniButtonRight))
      {
        list.DeleteArrayElementAtIndex(index);
        GUI.color = previousGUIColor;
        return false;
      }
      GUI.color = previousGUIColor;
      return true;
    }

  }
}