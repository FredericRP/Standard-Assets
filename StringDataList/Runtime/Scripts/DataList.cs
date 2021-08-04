using System.Collections.Generic;

namespace FredericRP.StringDataList
{
  /// <summary>
  /// Unfortunately, a drawer is called on each element of an array, that's why we have to encapsulate the list in the class we want to put the attribute on
  /// </summary>
  /// <typeparam name="T"></typeparam>
  [System.Serializable]
  public class DataList<T>
  {
    public List<T> list;

    // Define the indexer to allow client code to use [] notation.
    public T this[int i]
    {
      get { return ((list != null && i >= 0 && i < list.Count) ? list[i] : default(T)); }
      set { list[i] = value; }
    }

    public int Count { get { return (list == null) ? 0 : list.Count; } }
  }
}