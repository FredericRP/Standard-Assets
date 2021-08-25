using System.IO;
using UnityEngine;

namespace FredericRP.PersistentData
{
  [System.Serializable]
  public class SavedData
  {
    /// <summary>
    /// If your serialized class implement this interface, you can control all the serialization process.
    /// </summary>
    public interface IFullSerializationControl
    {
      /// <summary>
      /// Save Data to a file
      /// </summary>
      /// <param name="writer"></param>
      void WriteObjectData(BinaryWriter writer);

      /// <summary>
      /// Construct your object from a file
      /// </summary>
      /// <param name="reader"></param>
      void ReadObjectData(BinaryReader reader);
    }

    /// <summary>
    /// The current file path for this saved data.
    /// Null if no file is associed to the data (it is not already save in a file).
    /// Can change at any time.
    /// Do not modify this manually.
    /// </summary>
    [System.NonSerialized]
    internal string filePath = null;

    /// <summary>
    /// The file number associed to this saved data.
    /// Can change at any time.
    /// Do not modify this manually.
    /// </summary>
    [System.NonSerialized]
    public int fileNumber = -1;

    public string dataName = "SavedData";

    public string dataVersion;

    /// <summary>
    /// Function called when OnDatacreated is call by persistentData.
    /// This is called when no data is detected in default saved data or in player saved data.
    /// </summary>
    /// <param name="dataVersion"></param>
    public virtual void onDataCreated(string dataVersion) { }

    /// <summary>
    /// Called when default saved data are loaded (when there is no player saved data and there is a default saved data file)
    /// </summary>
    public virtual void onDefaultDataLoaded() { }

    /// <summary>
    /// Called when the persistentDataSystem version do not correspond to the savedData version
    /// </summary>
    /// <param name="dataVersion"></param>
    public virtual void onChangeDataVersion(string dataVersion) { }
  }
}
