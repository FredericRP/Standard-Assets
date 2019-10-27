using FredericRP.AssetBundleTool;
using FredericRP.BucketGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.Tips
{
  public class TipsLoader : Singleton<TipsLoader>
  {
    [SerializeField]
    string assetBundleName = null;
    [SerializeField]
    string assetName = null;

    string[] tipList;
    Bucket bucket;

    // Start is called before the first frame update
    void Start()
    {
      AssetBundle bundle = LocalLoader.LoadBundle(assetBundleName);
      if (bundle != null)
      {
        TextAsset tipsFile = bundle.LoadAsset<TextAsset>(assetName);
        if (tipsFile != null)
        {
          tipList = tipsFile.text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
          bucket = new Bucket(tipList.Length);
        } else
        {
          Debug.LogWarning("No TextAsset <" + assetName + "> found in bundle <" + assetBundleName + ">");
        }
      }
      else
      {
        Debug.LogWarning("No asset bundle <" + assetBundleName + "> found !");
      }
    }

    /// <summary>
    /// Get next random tip from loaded text file
    /// </summary>
    /// <returns></returns>
    public string GetTip()
    {
      if (tipList != null && tipList.Length > 0)
        return tipList[bucket.GetRandomNumber()];
      return null;
    }

  }
}