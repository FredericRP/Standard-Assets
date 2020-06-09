using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public class DependencyUpdater : MonoBehaviour
{

  [System.Serializable]
  class ScopedRegistryJson
  {
    public string name;
    public string url;
    public List<string> scopes;
  }
  [System.Serializable]
  internal class CustomPackageJson
  {
    public string name;
    public string version;
    public string[] customDependencies;
  }

  static string ManifestFilePath => $"{Directory.GetParent(Application.dataPath)}\\Packages\\manifest.json";

  static DependencyUpdater()
  {
    EditorApplication.update += LaunchUpdate;
  }

  static void LaunchUpdate()
  {
    EditorApplication.update -= LaunchUpdate;

    ListRequest packageListRequest = Client.List(true);
    while (packageListRequest.Status == StatusCode.InProgress)
    {
      // wait for the list to be gathered
    }
    if (packageListRequest.Status == StatusCode.Success)
    {
      var packageToAddList = new List<string>();
      var frpPackageList = new List<UnityEditor.PackageManager.PackageInfo>();
      frpPackageList.AddRange(packageListRequest.Result.Where(package => package.name.StartsWith("com.fredericrp.")));
      // Read each package
      for (int i = 0; i < frpPackageList.Count; i++)
      {
        // Check if custom dependency is already present
        UnityEditor.PackageManager.PackageInfo packageInfo = frpPackageList[i];
        // - locate file
        //Debug.Log("Package " + packageInfo.displayName + " is located @ <" + packageInfo.resolvedPath + ">");
        string packageFilePath = packageInfo.resolvedPath + "\\package.json";
        if (File.Exists(packageFilePath))
        {
          Debug.Log("Reading...");
          CustomPackageJson customPackage = JsonUtility.FromJson<CustomPackageJson>(File.ReadAllText(packageFilePath));
          Debug.Log("Custom Package [" + customPackage.name + "] # " + customPackage.version);
          // - read custom dependencies
          if (customPackage.customDependencies != null)
          {
            Debug.Log(customPackage.customDependencies.Length + " package(s)");
            // Ask if user wants to import packages
            if (!EditorUtility.DisplayDialog("Import package dependencies ?", "The imported package(" + customPackage.name + ") has " + customPackage.customDependencies.Length + " custom dependency/ies. Would you like to import them automatically ?\nPlease ensure you have an active internet connection, the process can not be stopped", "Yes", "No"))
            {
              Debug.LogWarning("Automatic import of dependencies canceled.");
              //  You can import custom dependencies using Window/FredericRP/Import custom dependencies"
              return;
            }
            // (otherwise, allow this script to be launched afterwards
            for (int j = 0; j < customPackage.customDependencies.Length; j++)
            {
              Debug.Log("Package " + customPackage.customDependencies[j]);
              // - split name and version
              string[] dependencyInfo = customPackage.customDependencies[j].Split('#');
              // - check if present
              if (frpPackageList.Find(package => package.name.Equals(dependencyInfo[0]) && package.version.Equals(dependencyInfo[1])) == null)
              {
                // If not, add it to the list of packages to add
                packageToAddList.Add(customPackage.customDependencies[j]);
              }
            }
            // */
          }
        }
      }
      // Launch an AddRequest for each new dependency
      AddRequest addRequest;
      int step = 100 / packageToAddList.Count;
      int progress = 0;
      EditorUtility.DisplayProgressBar("Adding dependencies", "Retrieving " + packageToAddList.Count + " package(s)", 0 / 100f);
      for (int i = 0; i < packageToAddList.Count; i++)
      {
        Debug.Log("> Launching add request for " + packageToAddList[i]);
        addRequest = Client.Add(packageToAddList[i]);
        progress += step;
        progress = Mathf.Clamp(progress, 0, 100);
        EditorUtility.DisplayProgressBar("Adding dependencies", "Retrieving [" + packageToAddList[i] + "]", progress / 100f);
        while (addRequest.Status == StatusCode.InProgress) { };
        Debug.Log("< AddRequest status:" + addRequest.Status);
        if (addRequest.Status == StatusCode.Failure) {
          Debug.LogError("Adding package result: " + addRequest.Error.errorCode + ":" + addRequest.Error.message);
        }
      }
      EditorUtility.ClearProgressBar();
    }
    else
    {
      Debug.Log("Failed");
    }
  }
}