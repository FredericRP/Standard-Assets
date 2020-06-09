using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FredericRP.AssetBundleTool
{
  public class LocalLoader
  {
    public static AssetBundle LoadBundle(string assetBundleName)
    {
      var loadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
      if (loadedAssetBundle == null)
      {
        Debug.Log(string.Format("Failed to load AssetBundle {0} !", assetBundleName));
        return null;
      }
      return loadedAssetBundle;
    }
  }
}