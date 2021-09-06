using System;
using UnityEditor;
using UnityEngine;

namespace FredericRP.GameQuest
{
  [CustomPropertyDrawer(typeof(GameQuestInfo))]
  public class GameQuestInfoDrawer : PropertyDrawer
  {
    string[] dayList;
    string[] monthList;
    string[] yearChoiceList;
    string[] durationUnitList;

    GUIContent rewardLabel;

    //*
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // Using BeginProperty / EndProperty on the parent property means that
      // prefab override logic works on the entire property.
      EditorGUI.BeginProperty(position, label, property);

      /// 1.game quest ID and localization ID
      /// 2. Specific date day/all month/all [all]year
      /// 3. Duration (sec)
      /// 4. Target [amount] / [type id]

      // - line 1 : IDs
      Rect valueRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("ID"));
      valueRect.height = EditorGUIUtility.singleLineHeight;
      SerializedProperty gameQuestId = property.FindPropertyRelative("gameQuestID");
      SerializedProperty localizationId = property.FindPropertyRelative("localizationId");
      EditorGUI.PropertyField(valueRect, gameQuestId, GUIContent.none);
      // - for now, use same ID for localization and game quest
      // TODO: check conflicts with Localization systems
      localizationId.stringValue = gameQuestId.stringValue;
      // - line 2: date
      position.y += EditorGUIUtility.singleLineHeight;
      Rect dateRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Trigger date"));
      dateRect.height = EditorGUIUtility.singleLineHeight;
      var dayRect = new Rect(dateRect.x, dateRect.y, dateRect.width * 0.25f, dateRect.height);
      // Don't make child fields be indented
      var indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel--;
      // -- DAY IN MONTH
      SerializedProperty dayInMonth = property.FindPropertyRelative("dayInMonth");
      if (dayList == null)
      {
        dayList = new string[32];
        dayList[0] = "all";
        for (int dayNumber = 1; dayNumber <= 31; dayNumber++)
          dayList[dayNumber] = dayNumber.ToString();
      }
      dayInMonth.intValue = EditorGUI.Popup(dayRect, dayInMonth.intValue, dayList);
      // -- MONTH
      SerializedProperty month = property.FindPropertyRelative("month");
      SerializedProperty year = property.FindPropertyRelative("year");
      float yearWidth = dateRect.width * 0.75f - 125;
      var monthRect = new Rect(dateRect.x + dateRect.width * 0.25f + 5, dateRect.y, 70 + (year.intValue == 0 ? yearWidth : 0), dateRect.height);
      if (monthList == null)
      {
        monthList = new string[13];
        monthList[0] = "all";
        DateTime monthTemp = new DateTime(2021, 1, 1);
        for (int monthNumber = 1; monthNumber <= 12; monthNumber++)
        {
          monthList[monthNumber] = monthTemp.ToString("MMMM \t(M)");
          monthTemp = monthTemp.AddMonths(1);
        }
      }
      month.intValue = EditorGUI.Popup(monthRect, month.intValue, monthList);
      // -- YEAR
      var yearChoiceRect = new Rect(monthRect.x + 75 + (year.intValue == 0 ? yearWidth : 0), dateRect.y, 40, dateRect.height);
      var yearIntRect = new Rect(yearChoiceRect.x + 45, dateRect.y, yearWidth, dateRect.height);
      if (yearChoiceList == null)
      {
        yearChoiceList = new string[2];
        yearChoiceList[0] = "all";
        yearChoiceList[1] = "set";
      }
      int yearChoice = EditorGUI.Popup(yearChoiceRect, year.intValue == 0 ? 0 : 1, yearChoiceList);
      if (yearChoice == 0)
      {
        year.intValue = 0;
      }
      else
      {
        year.intValue = EditorGUI.IntField(yearIntRect, year.intValue);
        // Can not set a year in the past
        if (year.intValue < DateTime.Now.Year)
          year.intValue = DateTime.Now.Year;
      }
      // - line 3 : duration
      position.y += EditorGUIUtility.singleLineHeight;
      Rect durationRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Duration"));
      durationRect.height = EditorGUIUtility.singleLineHeight;
      durationRect.width -= 75;
      Rect durationUnitRect = new Rect(durationRect.x + durationRect.width + 5, durationRect.y, 70, durationRect.height);
      if (durationUnitList == null)
      {
        durationUnitList = new string[3];
        durationUnitList[0] = "second(s)";
        durationUnitList[1] = "minute(s)";
        durationUnitList[2] = "hour(s)";
      }
      SerializedProperty duration = property.FindPropertyRelative("duration");
      // > uses modulo and size to find best unit
      // > if modulo 3600 gives 0, hours is fine
      // > if modulo 60 gives 0, minutes is fine
      // > uses seconds otherwise
      int usedValue = duration.intValue;
      int durationUnit = 0;
      if (usedValue % 3600 == 0)
      {
        usedValue /= 3600;
        durationUnit = 2;
      }
      else if (usedValue % 60 == 0)
      {
        usedValue /= 60;
        durationUnit = 1;
      }
      usedValue = EditorGUI.IntField(durationRect, usedValue);
      durationUnit = EditorGUI.Popup(durationUnitRect, durationUnit, durationUnitList);
      duration.intValue = usedValue * (durationUnit == 2 ? 3600 : durationUnit == 1 ? 60 : 1);
      // - line 4 : target amount and type
      position.y += EditorGUIUtility.singleLineHeight;
      Rect targetRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Target (amount/type)"));
      targetRect.height = EditorGUIUtility.singleLineHeight;
      var targetAmountRect = new Rect(targetRect.x, targetRect.y, targetRect.width * 0.25f, dateRect.height);
      var targetIdRect = new Rect(targetRect.x + targetRect.width * 0.25f + 5, targetRect.y, targetRect.width * 0.75f - 5, targetRect.height);
      SerializedProperty target = property.FindPropertyRelative("target");
      EditorGUI.PropertyField(targetAmountRect, target, GUIContent.none);
      SerializedProperty targetId = property.FindPropertyRelative("targetId");
      EditorGUI.PropertyField(targetIdRect, targetId, GUIContent.none);

      // line 5: Reward list
      if (rewardLabel == null)
        rewardLabel = new GUIContent("Rewards");
      position.y += EditorGUIUtility.singleLineHeight;
      Rect rewardRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), rewardLabel);
      rewardRect.height = EditorGUIUtility.singleLineHeight;
      SerializedProperty gameQuestRewardList = property.FindPropertyRelative("gameQuestRewardList");
      EditorGUI.PropertyField(rewardRect, gameQuestRewardList, GUIContent.none);

      // Set indent back to what it was
      EditorGUI.indentLevel = indent;
      // Apply modified changes
      property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();

    }
    // */
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      float height = EditorGUIUtility.singleLineHeight * 5;
      SerializedProperty gameQuestRewardList = property.FindPropertyRelative("gameQuestRewardList");
      if (!gameQuestRewardList.isExpanded)
        height += EditorGUIUtility.singleLineHeight;
      else
      {
        height += (3 + gameQuestRewardList.arraySize) * EditorGUIUtility.singleLineHeight;
        //height += base.GetPropertyHeight(gameQuestRewardList, rewardLabel);
      }
      return height;
    }
  }
}