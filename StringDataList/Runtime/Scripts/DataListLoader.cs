using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.StringDataList
{
  public class DataListLoader : MonoBehaviour
  {

    public static List<string> GetDataList(string identifier)
    {
      List<string> seasonList = new List<string>();
      // Load the asset from resources
      TextAsset text = Resources.Load<TextAsset>("datalist/" + identifier);
      if (text != null)
      {
        seasonList.AddRange(text.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries));
      }
      // Return the season list
      return seasonList;
    }
  }
}