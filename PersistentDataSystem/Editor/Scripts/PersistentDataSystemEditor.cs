using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;
using System.IO;

namespace FredericRP.PersistentData
{
  [CustomEditor(typeof(PersistentDataSystem))]
  public class PersistentDataSystemEditor : UnityEditor.Editor
  {
    PersistentDataSystem.SaveType currentSaveType = PersistentDataSystem.SaveType.Default;

    static bool mustCheckList = true;

    [DidReloadScripts]
    public static void OnCompileScripts()
    {
      mustCheckList = true;
    }

    public override void OnInspectorGUI()
    {
      if (mustCheckList)
      {
        MonoScript ms = MonoScript.FromScriptableObject(this);
        UpdateSpecificClassList<SavedData>(ms, PersistentDataSystem.SpecificClassListDataFilename + ".txt");
      }

      serializedObject.Update();
      EditorGUI.BeginChangeCheck();
      PersistentDataSystem persistentDataSystem = ((PersistentDataSystem)target);

      GUILayout.Label("Version management", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("dataVersion"), true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("versionImporters"), new GUIContent("Previous versions importers"), true);

      EditorGUILayout.Space();
      GUILayout.Label("Load and Save behavior", EditorStyles.boldLabel);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("autoSave"), true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("saveMode"), true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("awakeLoadMode"), true);

      if (persistentDataSystem.awakeLoadMode == PersistentDataSystem.AwakeLoadMode.SpecificClass)
      {
        SpecificClassEditor(persistentDataSystem);
      }

      EditorGUILayout.Space();
      GUILayout.Label("Data", EditorStyles.boldLabel);
      // Data type (default/player), then load/save/erase
      EditorGUILayout.BeginHorizontal();
      currentSaveType = (PersistentDataSystem.SaveType)EditorGUILayout.EnumPopup(currentSaveType);
      if (GUILayout.Button("Load", EditorStyles.miniButtonLeft))
        LoadCurrentSaveType(persistentDataSystem);
      if (GUILayout.Button("Save", EditorStyles.miniButtonMid))
        SaveCurrentSaveType(persistentDataSystem);
      Color previousColor = GUI.color;
      GUI.color = Color.red;
      if (GUILayout.Button("Erase", EditorStyles.miniButtonRight))
        EraseCurrentSaveType(persistentDataSystem);
      GUI.color = previousColor;
      EditorGUILayout.EndHorizontal();

      GUILayout.Space(10);
      if (GUILayout.Button("Unload saved data"))
        persistentDataSystem.UnloadAllSavedData();

      GUILayout.Space(10);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("debug"), true);

      EditorGUILayout.Space();
      GUILayout.Label("Loaded Data", EditorStyles.boldLabel);
      persistentDataSystem.Init();

      if (persistentDataSystem.savedDataList != null)
      {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("savedDataList"), true);
      }

      serializedObject.ApplyModifiedProperties();
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Persistent Data System");
      }
    }

    /// <summary>
    /// Get types derived from <c>SavedData</c> and update the pds-specific-class data list for the editor to find them
    /// </summary>
    public static void UpdateSpecificClassList<T>(MonoScript source, string dataListFilename)
    {
      string dataPath;      
      dataPath = AssetDatabase.GetAssetPath(source);

      FileInfo fi = new FileInfo(dataPath);
      // Get parent, then add Resources/datafile/pds-specific-class.txt to get the correct data path
      string dataDirectory = fi.Directory.Parent.ToString().Replace('\\', '/') + "/Resources/datalist";
      // ensure directory exists
      if (!File.Exists(dataDirectory))
        Directory.CreateDirectory(dataDirectory);
      dataPath = dataDirectory + "/" + dataListFilename;

      using (FileStream fs = File.Create(dataPath))
      {
        StreamWriter writer = new StreamWriter(fs);

        var extractedTypes = TypeCache.GetTypesDerivedFrom<T>();
        foreach (Type type in extractedTypes)
        {
          if (type.IsSubclassOf(typeof(T)))
          {
            writer.Write(type.FullName + "\n");
          }
        }
        writer.Close();
        fs.Close();
      }
      mustCheckList = false;
    }

    void SpecificClassEditor(PersistentDataSystem persistentDataSystem)
    {
      SerializedProperty classToLoad = serializedObject.FindProperty("classToLoad");
      EditorGUILayout.PropertyField(classToLoad, new GUIContent("Specific classes"));
    }

    public static string GetPropertyType(SerializedProperty property)
    {
      var type = property.type;
      var match = Regex.Match(type, @"PPtr<\$(.*?)>");
      if (match.Success)
        type = match.Groups[1].Value;
      return type;
    }

    void LoadCurrentSaveType(PersistentDataSystem persistentDataSystem)
    {
      if (persistentDataSystem.awakeLoadMode == PersistentDataSystem.AwakeLoadMode.SpecificClass)
        persistentDataSystem.LoadClass(persistentDataSystem.classToLoad, currentSaveType);
      else
        persistentDataSystem.LoadAllSavedData(currentSaveType);
    }

    void EraseCurrentSaveType(PersistentDataSystem persistentDataSystem)
    {
      persistentDataSystem.EraseAllSavedData(currentSaveType);
      AssetDatabase.Refresh(ImportAssetOptions.Default);
    }

    void SaveCurrentSaveType(PersistentDataSystem persistentDataSystem)
    {
      persistentDataSystem.SaveAllData(currentSaveType);
      AssetDatabase.Refresh(ImportAssetOptions.Default);
    }
  }
}