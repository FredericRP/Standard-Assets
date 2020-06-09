using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.AssetBundleTool
{
  public class AssetBundleExportWindow : EditorWindow
  {
    const string EDITOR_PREFS_KEY = "FredericRP.AssetBundlesExporter.ExportDirectory";

    string assetBundleDirectory = "Assets/AssetBundles";
    Vector2 windowMinSize = new Vector2(340, 80);
    Vector2 windowMaxSize = new Vector2(640, 380);

    [MenuItem("Assets/FredericRP/Asset Bundles Build")]
    static void ShowWindow()
    {
      AssetBundleExportWindow window = GetWindow<AssetBundleExportWindow>();
      // Set min, max size and default size
      window.minSize = window.windowMinSize;
      window.maxSize = window.windowMaxSize;
      Rect position = window.position;
      position.width = window.windowMinSize.x;
      position.height = window.windowMinSize.y;
      window.position = position;
      window.ShowPopup();
    }

    private void OnEnable()
    {
      // Set export directory from editor prefs
      assetBundleDirectory = EditorPrefs.GetString(EDITOR_PREFS_KEY, "Assets/AssetBundles");
    }

    private void OnDisable()
    {
      // Store to editor prefs the export directory
      // TODO : make a per project save file ?
      EditorPrefs.SetString(EDITOR_PREFS_KEY, assetBundleDirectory);
    }

    private void OnGUI()
    {
      // Title
      titleContent = new GUIContent("Asset Bundle Build");
      // Description
      EditorGUILayout.HelpBox("Enter the output directory, then click on Export to create all asset bundles present in your unity project.", MessageType.Info);
      // Directory
      assetBundleDirectory = ShowDirectoryGUI(assetBundleDirectory);
      // Build / Cancel button
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("BUILD"))
      {
        Build();
      }
      if (GUILayout.Button("CANCEL"))
      {
        Close();
      }
      GUILayout.EndHorizontal();
    }

    string ShowDirectoryGUI(string directory)
    {
      return GUILayout.TextField(directory);
    }

    void Build()
    {
      if (!Directory.Exists(assetBundleDirectory))
      {
        Directory.CreateDirectory(assetBundleDirectory);
      }
      BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                      BuildAssetBundleOptions.None,
                                      BuildTarget.StandaloneWindows);
      // close once it's done
      Close();
    }
  }
}