using UnityEngine;

namespace FredericRP.StringDataList
{
  public class SelectAttribute : PropertyAttribute
  {
    public string identifier;

    public SelectAttribute(string identifier)
    {
      this.identifier = identifier;
    }
  }
}