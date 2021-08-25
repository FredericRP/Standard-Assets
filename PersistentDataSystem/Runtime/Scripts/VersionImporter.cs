using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.PersistentData
{
  public abstract class VersionImporter : MonoBehaviour
  {

    [SerializeField]
    protected string importVersion;

    [SerializeField]
    protected string targetVersion;

    public string InternImport(SavedData savedData)
    {
      string baseVersion = savedData.dataVersion;

      savedData.dataVersion = Import(savedData);

      if (!baseVersion.Equals(savedData.dataVersion))
      {
        savedData.onChangeDataVersion(savedData.dataVersion);
      }

      return savedData.dataVersion;
    }

    /// <summary>
    /// Change information in the savedDatas list for importing data.
    /// Be carefull to test if import is possible.
    /// </summary>
    /// <param name="currentVersion"></param>
    /// <param name="savedDatas"></param>
    /// <returns>the number of the new version</returns>
    public abstract string Import(SavedData savedDatas);

    /// <summary>
    /// Return true, if the current version correspond to the version which can be import
    /// </summary>
    /// <param name="currentVersion"></param>
    /// <returns></returns>
    public bool CanImport(string currentVersion)
    {
      return currentVersion.Equals(importVersion);
    }
  }
}
