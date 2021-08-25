using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;
using FredericRP.StringDataList;
using FredericRP.GenericSingleton;

namespace FredericRP.PersistentData
{
  public class PersistentDataSystem : Singleton<PersistentDataSystem>
  {
    public enum AwakeLoadMode { None, SpecificClass, AllSavedData };
    public enum SaveMode { SingleFile, MultipleFile };
    public enum SaveType { Default, Player };
    /// <summary>
    /// Folder where PersistentDataSystem files are stored
    /// </summary>
    public const string PersistentDataSystemDirectory = "PersistentDataSystem";

    /// <summary>
    /// The directory name used when automatic naming is using
    /// </summary>
    public const string AutomaticDirectoryName = "Automatic";

    /// <summary>
    /// The directory where is stocked the file for single file mode
    /// </summary>
    public const string SingleFileDirectoryName = "SingleFile";

    /// <summary>
    /// The name a the file used for the one file mode
    /// </summary>
    public const string SingleFileName = "savedData";

    /// <summary>
    /// The directory where is stocked the file for multiple files mode
    /// </summary>
    public const string MultipleFilesDirectoryName = "MultipleFiles";

    /// <summary>
    /// Extension of file that the system use for automatic (ie use of BinaryFormatter) serialization
    /// </summary>
    public const string AutomaticSerializationFileExtension = ".apds";

    /// <summary>
    /// Extension of file that the system use for controlled (ie use of StreamWriter) serialization
    /// </summary>
    public const string ControlledSerializationFileExtension = ".cpds";


    /// <summary>
    /// Called when a data is saved to a file
    /// </summary>
    public static Action<SavedData> OnDataSaved;

    /// <summary>
    /// Called when the data is loaded from a file
    /// </summary>
    public static Action<SavedData> OnDataLoaded;

    [Tooltip("Identify the current (latest) version of the persistentData")]
    public string dataVersion = "v0";

    [Tooltip("Version importer, successive importer are applied in the list order, from index 0")]
    public VersionImporter[] versionImporters;

    [Tooltip("Auto save on application pause and quit")]
    public bool autoSave = true;

    [Tooltip("Behavior on awake")]
    public AwakeLoadMode awakeLoadMode = AwakeLoadMode.AllSavedData;

    [Tooltip("One big file or multiple files, meaning one file for each data object. It allows:\n1- Saved data add-on easily done\n2- Partial data loading\n3- One file corruption do not impact the entire saved data")]
    public SaveMode saveMode = SaveMode.MultipleFile;

    [Tooltip("Classes to load at the start of the game")]
    [Select("pds-specific-class")]
    public List<string> classToLoad;

    [SerializeReference]
    public List<SavedData> savedDataList = new List<SavedData>();
    [NonSerialized]
    public List<bool> savedDataFoldout = new List<bool>();

    /// <summary>
    /// For automatic saving (for the moment we only use this)
    /// </summary>
    public string automaticPlayerSavedDataDirectoryPath { get; private set; }
    public string singlePlayerFileDirectoryPath { get; private set; }
    public string multiplePlayerFilesDirectoryPath { get; private set; }

    /// <summary>
    /// For saving default savedData
    /// </summary>
    public string singleDefaultFileDirectoryPath { get; private set; }
    public string multipleDefaultFilesDirectoryPath { get; private set; }

    private bool isInit = false;
    public bool IsInit
    {
      get { return isInit; }
    }

#if UNITY_EDITOR
    [Space(10)]
    public bool debug = true;
#endif

    public override void OnAwake()
    {
      base.OnAwake();

      // Ensure there is no previously serialized data, we load it from the file only (serialization is there so Unity can display it in the inspector)
      savedDataList?.Clear();

      if (!isInit)
        Init();

      switch (awakeLoadMode)
      {
        case AwakeLoadMode.SpecificClass: LoadClass(this.classToLoad); break;
        case AwakeLoadMode.AllSavedData: LoadAllSavedData(); break;
        default: break;
      }
    }

    /// <summary>
    /// Init the PersistentData system, important for the editor, used in the Awake function
    /// </summary>
    public void Init()
    {
      automaticPlayerSavedDataDirectoryPath = System.IO.Path.Combine(System.IO.Path.Combine(Application.persistentDataPath, PersistentDataSystemDirectory), AutomaticDirectoryName);
      singlePlayerFileDirectoryPath = System.IO.Path.Combine(automaticPlayerSavedDataDirectoryPath, SingleFileName);
      multiplePlayerFilesDirectoryPath = System.IO.Path.Combine(automaticPlayerSavedDataDirectoryPath, MultipleFilesDirectoryName);

      singleDefaultFileDirectoryPath = System.IO.Path.Combine(System.IO.Path.Combine(Application.streamingAssetsPath, PersistentDataSystemDirectory), SingleFileName);
      multipleDefaultFilesDirectoryPath = System.IO.Path.Combine(System.IO.Path.Combine(Application.streamingAssetsPath, PersistentDataSystemDirectory), MultipleFilesDirectoryName);

      isInit = true;
    }

    void Reset()
    {
      OnDataSaved = null;
      OnDataLoaded = null;

      if (classToLoad != null)
        classToLoad.Clear();

      if (savedDataList != null)
        savedDataList.Clear();
    }

    /// <summary>
    /// Create new instance of class and load it with the LoadSavedData function. Independent of multiple files or not.
    /// Will "destroy" all previous loaded data from persistentData dictionnary
    /// </summary>
    /// <param name="classToLoad"></param>
    /// <param name="saveType">if defined on LoadMode.Default, it will load default saved data in the persistentData folder</param>
    /// <returns></returns>
    public bool LoadClass(List<string> classToLoad, SaveType saveType = SaveType.Player)
    {
      if (classToLoad == null)
      {
        Debug.LogError("Class empty : can not load data");
        return false;
      }

      savedDataList.Clear();

      if (saveMode == SaveMode.SingleFile)
      {
        LoadAllSavedData(saveType);
      }
      else
      {
        foreach (string i in classToLoad)
        {
          Type type = Type.GetType(i);

          LoadSavedData(type, saveType);
        }
      }

      return true;
    }


    /// <summary>
    /// Load all the saved data in the persistentDataSystem directory.
    /// </summary>
    public void LoadAllSavedData(SaveType saveType = SaveType.Player, bool clearExistingData = false)
    {
      if (clearExistingData)
        savedDataList?.Clear();

      //The directory does not exist, return immediatly
      if ((!Directory.Exists(singleDefaultFileDirectoryPath) && saveMode == SaveMode.SingleFile) || (!Directory.Exists(multipleDefaultFilesDirectoryPath) && saveMode == SaveMode.MultipleFile))
      {
        return;
      }

      if (saveMode == SaveMode.MultipleFile)
      {
        string[] directories;

        if (saveType == SaveType.Player)
          directories = Directory.GetDirectories(multiplePlayerFilesDirectoryPath);
        else
          directories = Directory.GetDirectories(multipleDefaultFilesDirectoryPath);

        for (int i = 0; i < directories.Length; i++)
        {
          string[] files = Directory.GetFiles(directories[i], "*", SearchOption.AllDirectories);

          for (int j = 0; j < files.Length; j++)
          {
            SavedData savedData = LoadSavedData(files[j], j, saveType == SaveType.Default);

            if (savedData != null)
            {
              savedDataList.Add(savedData);

              OnDataLoaded?.Invoke(savedData);
            }
          }
        }
      }
      else
      {
        try
        {
          string dataPath;

          if (saveType == SaveType.Player)
            dataPath = singlePlayerFileDirectoryPath + SingleFileName + AutomaticSerializationFileExtension;
          else
            dataPath = singleDefaultFileDirectoryPath + SingleFileName + AutomaticSerializationFileExtension;

          BinaryFormatter bf = new BinaryFormatter();
          using (FileStream fs = File.Open(dataPath, FileMode.Open, FileAccess.Read))
          {
            savedDataList = (List<SavedData>)bf.Deserialize(fs);
            fs.Close();
          }

          //If version is different, reinit with default file
          for (int i = 0; i < savedDataList.Count; i++)
          {
            if (!CheckDataversion(dataVersion, savedDataList[i]))
            {
              if (saveType == SaveType.Player)
              {
                LoadAllSavedData(SaveType.Default);
                return;
              }
              else
                return;// new List<SavedData>(); //Considere Data are all outdated
            }

            savedDataList.Add(savedDataList[i]);
          }
        }
        catch
        {
#if UNITY_EDITOR
          if (debug)
            Debug.LogWarning("Unable to load persistent data");
#endif
        }
      }
    }

    /// <summary>
    /// Load saved data of the specified type, if no such file exist, one saved data will be Init.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>null is used with single file system<</returns>
    SavedData LoadSavedData(Type type, SaveType saveType = SaveType.Player)
    {
      if (saveMode == SaveMode.SingleFile)
        return null;

      SavedData savedData = null;
      ClearSavedDataList(type);

      try
      {
        string dataPath;

        if (saveType == SaveType.Player)
          dataPath = System.IO.Path.Combine(System.IO.Path.Combine(multiplePlayerFilesDirectoryPath, type.Name), type.Name);
        else
          dataPath = System.IO.Path.Combine(System.IO.Path.Combine(multipleDefaultFilesDirectoryPath, type.Name), type.Name);

        if (type.GetInterface(typeof(SavedData.IFullSerializationControl).Name) != null)
          dataPath += ControlledSerializationFileExtension;
        else
          dataPath += AutomaticSerializationFileExtension;

        Debug.Log("Loading " + dataPath);
        savedData = LoadSavedData(dataPath, 0, saveType == SaveType.Default);

        if (savedData == null)
        {
          throw new System.ArgumentException("No data of this type or incompatible version");
        }
        else if (saveType == SaveType.Default)
          savedData.onDefaultDataLoaded();
      }
      catch
      {
        Debug.Log(saveType);

        if (saveType == SaveType.Player)
          return LoadSavedData(type, SaveType.Default);

        //savedData = (SavedData)ScriptableObject.CreateInstance(type);
        savedData = (SavedData)Activator.CreateInstance(type);
        savedData.dataVersion = dataVersion;
        savedData.onDataCreated(dataVersion);

#if UNITY_EDITOR
        if (debug)
          Debug.Log("New instance of " + type + " loaded (" + savedData + ")");
#endif

        savedDataList.Add(savedData);

        if (OnDataLoaded != null)
          OnDataLoaded(savedData);
      }

      return savedData;
    }

    /// <summary>
    /// Load the first saved data of type T, if no such file exist, saved data will be Init.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>null if used with single file system</returns>
    public T LoadSavedData<T>(SaveType saveType = SaveType.Player) where T : SavedData
    {
      return (T)LoadSavedData(typeof(T), saveType);
    }


    /// <summary>
    /// Load all saved data of the type. If no savedData of this type exist, this will NOT create a new one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maxNumber"></param>
    /// <returns></returns>
    public List<T> LoadAllSavedData<T>(int maxNumber = int.MaxValue, SaveType saveType = SaveType.Player) where T : SavedData
    {
      if (saveMode == SaveMode.SingleFile)
        return null;

      List<T> savedDataList = new List<T>();
      SavedData savedData = null;
      Type type = typeof(T);
      ClearSavedDataList(type);

      for (int i = 0; i < maxNumber; i++)
      {
        try
        {
          string playerDataPath;
          string defaultDataPath;

          playerDataPath = System.IO.Path.Combine(System.IO.Path.Combine(multiplePlayerFilesDirectoryPath, type.Name), i.ToString());
          defaultDataPath = System.IO.Path.Combine(System.IO.Path.Combine(multipleDefaultFilesDirectoryPath, type.Name), i.ToString());

          if (type.GetInterface(typeof(SavedData.IFullSerializationControl).Name) != null)
          {
            playerDataPath += ControlledSerializationFileExtension;
            defaultDataPath += ControlledSerializationFileExtension;
          }
          else
          {
            playerDataPath += AutomaticSerializationFileExtension;
            defaultDataPath += AutomaticSerializationFileExtension;
          }

          if (saveType == SaveType.Player)
            savedData = LoadSavedData(playerDataPath, i, false);
          else
            savedData = LoadSavedData(defaultDataPath, i, true);

          //If player data are not present or version is not the same, try with default
          if (savedData == null && saveType == SaveType.Player)
          {
            savedData = LoadSavedData(defaultDataPath, i, false);
          }

          if (savedData != null)
            savedDataList.Add((T)savedData);
          else
            break;
        }
        catch
        {
          break;
        }
      }

      return savedDataList;
    }

    /// <summary>
    /// Return a T type, find in data dictionnary or create
    /// This function will try to load the data if it is not already loaded
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetSavedData<T>(SaveType saveType = SaveType.Player) where T : SavedData, new()
    {
      List<SavedData> typeSavedDataList = savedDataList.FindAll(data => data.GetType() == typeof(T));
      //savedDataDictionnary.TryGetValue(typeof(T), out savedDataList);

      if (typeSavedDataList?.Count > 0)
      {
        object send = typeSavedDataList[0];
        send = System.Convert.ChangeType(send, typeof(T));

        return (T)send;
      }
      //This data is not in the dictionnary
      else
      {
        //It's not in the list, so if use multiple file LoadIt with the file
        if (saveMode == SaveMode.MultipleFile)
        {
          return LoadSavedData<T>(saveType);
        }
        //Create a new instance of T
        else
        {
          T newSavedData = new T();
          //savedDataList = new List<SavedData>();
          savedDataList.Add(newSavedData);
          //savedDataDictionnary.Add(typeof(T), savedDataList);

          return newSavedData;
        }
      }
    }

    /// <summary>
    /// Return a T type in the persistent data dictionnary.
    /// If T type is not present in the dictionnary, this function will try to load saved files.
    /// Load saved file work only for multiple file system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maxNumber"></param>
    /// <returns></returns>
    public List<T> GetAllSavedData<T>(int maxNumber = int.MaxValue, SaveType saveType = SaveType.Player) where T : SavedData, new()
    {
      List<SavedData> typeSavedDataList = savedDataList.FindAll(data => data.GetType() == typeof(T));
      //savedDataDictionnary.TryGetValue(typeof(T), out savedDataList);

      if (typeSavedDataList != null)
      {
        return typeSavedDataList as List<T>;
      }
      //This data is not in the dictionnary
      else
      {
        //It's not in the dictionnary, so if use multiple file LoadIt with the file
        if (saveMode == SaveMode.MultipleFile)
        {
          return LoadAllSavedData<T>(maxNumber, saveType);
        }
      }

      return new List<T>();
    }


    /// <summary>
    /// Save all data in the PersistentDataSystem dictionnary
    /// </summary>
    public void SaveAllData(SaveType pathMode = SaveType.Player)
    {
      if (saveMode == SaveMode.SingleFile)
      {
        string dataPath;

        if (pathMode == SaveType.Player)
          dataPath = singlePlayerFileDirectoryPath;
        else
          dataPath = singleDefaultFileDirectoryPath;

        if (!Directory.Exists(dataPath))
          Directory.CreateDirectory(dataPath);

        dataPath += SingleFileName + AutomaticSerializationFileExtension;

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fs = File.Create(dataPath))
        {
          bf.Serialize(fs, savedDataList);
          fs.Close();
        }

        foreach (SavedData sd in savedDataList)
        {
          sd.filePath = dataPath;
          sd.fileNumber = 0;
        }
      }
      else
      {
        // TODO remove files if a class to load has been removed but a previous file still exists
        foreach (SavedData sd in savedDataList)
        {
          SaveData(sd, pathMode);
        }
      }

      foreach (SavedData sd in savedDataList)
      {
        if (OnDataSaved != null)
          OnDataSaved(sd);
      }
    }

    /// <summary>
    /// Only save the first instance of the specify class
    /// doesn't work if not multiple file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void SaveData<T>(SaveType loadMode = SaveType.Player) where T : SavedData, new()
    {
      if (saveMode == SaveMode.SingleFile)
      {
        Debug.LogError("You don't use multiple file, you can't save only one class. Use SaveData() instead");
        return;
      }

      //The class is in persistent data, find it and save it
      T savedData = GetSavedData<T>();
      SaveData(savedData, loadMode);
    }


    /// <summary>
    /// Will save your data only if it present on persistentData
    /// </summary>
    /// <param name="savedData"></param>
    public void SaveData(SavedData savedData, SaveType saveType = SaveType.Player)
    {
      if (saveMode == SaveMode.SingleFile)
      {
        Debug.LogError("You don't use multiple file, you can't save only one saved data. Use SaveData() instead");
        return;
      }

      string dataPath = savedData.filePath;
      var type = savedData.GetType();

      if (saveType == SaveType.Player)
        dataPath = System.IO.Path.Combine(multiplePlayerFilesDirectoryPath, type.Name);
      else
        dataPath = System.IO.Path.Combine(multipleDefaultFilesDirectoryPath, type.Name);

      if (!Directory.Exists(dataPath))
        Directory.CreateDirectory(dataPath);

      dataPath += "/" + type.Name;// cpt;

      if (savedData is SavedData.IFullSerializationControl)
      {
        dataPath += ControlledSerializationFileExtension;
        Debug.Log("Saving " + dataPath);
        using (FileStream fs = File.Create(dataPath))
        {
          BinaryWriter writer = new BinaryWriter(fs);
          writer.Write(type.Name);
          writer.Write(savedData.dataVersion);

          ((SavedData.IFullSerializationControl)savedData).WriteObjectData(writer);
          fs.Close();
        }
      }
      else
      {
        dataPath += AutomaticSerializationFileExtension;
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("Serializing " + dataPath);
        using (FileStream fs = File.Create(dataPath))
        {
          bf.Serialize(fs, savedData);
          fs.Close();
        }
        Debug.Log("Done serializing " + dataPath);
      }

      savedData.filePath = dataPath;
    }


    /// <summary>
    /// This will add a type T to be saved on the next SaveData call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddNewSavedData<T>() where T : SavedData, new()
    {
      T newSavedData = new T();
      newSavedData.dataVersion = dataVersion;
      savedDataList.Add(newSavedData);
      newSavedData.onDataCreated(dataVersion);

      return newSavedData;
    }

    private void AddSavedDataToList(SavedData savedData)
    {
      savedDataList.Add(savedData);
    }

    private void ClearSavedDataList(Type type)
    {
      savedDataList.RemoveAll(data => data.GetType() == type);
    }

    /// <summary>
    /// Load a savedData with a filePath
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileNumber"></param>
    /// <returns>null if the data is not in the good version or does not exist</returns>
    private SavedData LoadSavedData(string filePath, int fileNumber, bool isInStreamingAsset)
    {
      SavedData savedData = null;

      try
      {
#if UNITY_ANDROID && !UNITY_EDITOR
                //Android is using .jar file, require another access !
                if (isInStreamingAsset)
                {
                    WWW wwwReader = new WWW(filePath);
                    while (!wwwReader.isDone) { };

                    if (filePath.EndsWith(AutomaticSerializationFileExtension))
                    {
                        BinaryFormatter bf = new BinaryFormatter();

                        using (MemoryStream ms = new MemoryStream(wwwReader.bytes))
                        {
                            savedData = bf.Deserialize(ms) as SavedData;
                            ms.Close();
                        }
                    }
                    else if (filePath.EndsWith(ControlledSerializationFileExtension))
                    {
                        using (MemoryStream ms = new MemoryStream(wwwReader.bytes))
                        {
                            BinaryReader reader = new BinaryReader(ms);
                            Type type = Type.GetType(reader.ReadString());
                            savedData = (SavedData)ScriptableObject.CreateInstance(type);
                            ((SavedData.IFullSerializationControl)savedData).SetObjectData(reader);
                            ms.Close();
                        }
                    }
                }
                else
                {
#endif
        if (filePath.EndsWith(AutomaticSerializationFileExtension))
        {
          BinaryFormatter bf = new BinaryFormatter();

          using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
          {
            savedData = bf.Deserialize(fs) as SavedData;
            fs.Close();
          }
        }
        else if (filePath.EndsWith(ControlledSerializationFileExtension))
        {
          using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
          {
            BinaryReader reader = new BinaryReader(fs);
            Type type = Type.GetType(reader.ReadString());
            savedData = (SavedData)Activator.CreateInstance(type);
            savedData.dataVersion = reader.ReadString();
            ((SavedData.IFullSerializationControl)savedData).ReadObjectData(reader);
            fs.Close();
          }
        }
#if UNITY_ANDROID && !UNITY_EDITOR
                }
#endif

        savedData.filePath = filePath;
        savedData.fileNumber = fileNumber;

        //If the version is corresponding, add the data to dictionnary
        if (CheckDataversion(dataVersion, savedData))
        {
          savedDataList.Add(savedData);

          if (OnDataLoaded != null)
            OnDataLoaded(savedData);

          return savedData;
        }

        return null;
      }
      catch
      {
        return null;
      }
    }

    bool CheckDataversion(string version, SavedData savedData)
    {
      if (savedData.dataVersion != version)
      {
        // Try to import with importers
        foreach (VersionImporter versionImporter in versionImporters)
        {
          if (versionImporter.CanImport(savedData.dataVersion))
            versionImporter.InternImport(savedData);
        }

        //The version is not the same after all importer, so this data version is not compatible with the current persistentDataSystem version
        if (savedData.dataVersion != version)
        {
#if UNITY_EDITOR
          if (debug)
            Debug.LogWarning("New version loaded for : " + savedData.GetType());
#endif
          // Create new SavedData
          savedData = (SavedData)Activator.CreateInstance(savedData.GetType());
          savedData.dataVersion = dataVersion;
          savedData.onDataCreated(dataVersion);

          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// This will erase all saved files
    /// </summary>
    public void EraseAllSavedData(SaveType saveType = SaveType.Player)
    {
      if (saveType == SaveType.Player)
      {
        if (Directory.Exists(automaticPlayerSavedDataDirectoryPath))
          Directory.Delete(automaticPlayerSavedDataDirectoryPath, true);
      }
      else
      {
        if (Directory.Exists(Application.streamingAssetsPath + "/" + PersistentDataSystemDirectory))
          Directory.Delete(Application.streamingAssetsPath + "/" + PersistentDataSystemDirectory, true);
      }

      savedDataList.Clear();
    }

    /// <summary>
    /// Erase all data of type T saved in the provided path
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="saveType"></param>
    public void EraseAllSavedData<T>(SaveType saveType = SaveType.Player) where T : SavedData
    {
      Type type = typeof(T);

      if (saveType == SaveType.Player)
      {
        if (Directory.Exists(System.IO.Path.Combine(multiplePlayerFilesDirectoryPath, type.Name)))
          Directory.Delete(System.IO.Path.Combine(multiplePlayerFilesDirectoryPath, type.Name), true);

        if (Directory.Exists(System.IO.Path.Combine(singlePlayerFileDirectoryPath, type.Name)))
          Directory.Delete(System.IO.Path.Combine(singlePlayerFileDirectoryPath, type.Name), true);
      }
      else
      {
        if (Directory.Exists(System.IO.Path.Combine(multipleDefaultFilesDirectoryPath, type.Name)))
          Directory.Delete(System.IO.Path.Combine(multipleDefaultFilesDirectoryPath, type.Name), true);

        if (Directory.Exists(System.IO.Path.Combine(singleDefaultFileDirectoryPath, type.Name)))
          Directory.Delete(System.IO.Path.Combine(singleDefaultFileDirectoryPath, type.Name), true);
      }

      savedDataList.RemoveAll(data => data.GetType() == typeof(T));
    }

    /// <summary>
    /// Unload saved data of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void UnloadSavedData<T>() where T : SavedData
    {
      ClearSavedDataList(typeof(T));
    }

    /// <summary>
    /// Unload saved data by clearing dictionnary
    /// </summary>
    public void UnloadAllSavedData()
    {
      savedDataList.Clear();
    }

    public void OnApplicationPause(bool paused)
    {

      if (!autoSave)
        return;

      if (paused && savedDataList != null)
      {
        SaveAllData();

#if UNITY_EDITOR
        Debug.LogWarning("Data Save On Pause By AUTO_SAVE_MANAGEMENT");
#endif
      }
#if UNITY_EDITOR
      else
        Debug.LogWarning("Data Not Save On Pause By AUTO_SAVE_MANAGEMENT");
#endif
    }

    public override void OnApplicationQuit()
    {
      if (autoSave && savedDataList != null)
      {
        SaveAllData();

#if UNITY_EDITOR
        Debug.LogWarning("Data Saved On Application Quit By AUTO_SAVE_MANAGEMENT");
#endif
        // Clear serialized data
        savedDataList?.Clear();
      }
      else
      {
#if UNITY_EDITOR
        Debug.LogWarning("Data Not Saved On Application Quit due to null data By AUTO_SAVE_MANAGEMENT");
#endif
      }
    }
  }
}