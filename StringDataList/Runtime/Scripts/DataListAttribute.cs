using UnityEngine;

namespace FredericRP.StringDataList
{
  public class DataListAttribute : PropertyAttribute
  {
    public readonly string identifier;

    public DataListAttribute(string identifier)
    {
      this.identifier = identifier;
    }
  }
}