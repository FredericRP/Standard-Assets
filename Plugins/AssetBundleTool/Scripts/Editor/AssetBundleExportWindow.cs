using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.AssetBundleTool
{
  public class AssetBundleExportWindow : EditorWindow
  {
    const string EDITOR_PREFS_KEY = "FredericRP.AssetBundlesExporter.ExportDirectory";

    string assetBundleDirectory;

    [MenuItem("Assets/FredericRP/Asset Bundles Build")]
    static void ShowWindow()
    {
      EditorWindow.CreateWindow<AssetBundleExportWindow>().ShowPopup();
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
    }
  }
}