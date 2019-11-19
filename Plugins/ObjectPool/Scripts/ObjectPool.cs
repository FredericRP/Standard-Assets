using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

namespace FredericRP.ObjectPooling
{
  [ExecuteInEditMode]
  [System.Serializable]
  public class ObjectPool : MonoBehaviour
  {
    /// <summary>
    /// An object pool can not be instantiated and is not a Singleton neither.
    /// Instead, you can use the GetObjectPool method to retrieve the one you want to use.
    /// It allows to have multiple pools in your game without having to link them manually
    /// </summary>
    #region Object Pool handling
    public string id = "pool";

    private static List<ObjectPool> objectPoolList;

    public static ObjectPool GetObjectPool(string id)
    {
      return objectPoolList.Find(element => element.id.Equals(id));
    }

    private void OnEnable()
    {
      if (objectPoolList == null)
        objectPoolList = new List<ObjectPool>();
      objectPoolList.Add(this);
    }
    private void OnDisable()
    {
      if (objectPoolList != null)
        objectPoolList.Remove(this);
    }

    protected ObjectPool() { }
    #endregion

    [System.Serializable]
    public class PoolGameObjectInfo
    {
      /// <summary>
      /// Identifier of the object
      /// </summary>
      public string id;
      /// <summary>
      /// The actual prefab to instantiate
      /// </summary>
      public GameObject prefab;
      /// <summary>
      /// Object instantiated when the object is created
      /// </summary>
      public int bufferCount = 10;
      /// <summary>
      /// Maximum amount of object of this type it can handle before destroying new ones
      /// </summary>
      public int maxCount = 10;
      public Transform defaultParent;
    }

    /// <summary>
    ///  THIS WAS ADDED TO COMPENSATE DESTROY(OBJ,TIME)
    /// </summary>
    public class PoolData
    {
      public PoolData(GameObject _obj, float _time)
      {
        gameObjectToPool = _obj;
        time = _time;
      }

      public GameObject gameObjectToPool;
      public float time;
    }

    /// <summary>
    /// The object prefabs which the pool can handle.
    /// </summary>
    [SerializeField]
    List<PoolGameObjectInfo> poolObjectList = new List<PoolGameObjectInfo>();

    public List<PoolGameObjectInfo> PoolGameObjectInfoList { get { return poolObjectList; } }

    /// <summary>
    /// The pooled objects currently available.
    /// </summary>
    private Dictionary<string, List<GameObject>> pooledObjects = new Dictionary<string, List<GameObject>>();

    List<PoolData> poolDatas = new List<PoolData>();

    void Update()
    {
      foreach (PoolData i in poolDatas)
      {
        if (Time.time > i.time)
        {
          Pool(i.gameObjectToPool);
        }
      }
    }

    public int LoopCount()
    {
      int count = 0;
      foreach (PoolGameObjectInfo poolObject in poolObjectList)
      {
        count += poolObject.bufferCount;
      }
      return count;
    }

    void Awake()
    {
      poolDatas.Clear();
      pooledObjects.Clear();
      HeavyAwake();
    }

    public void HeavyAwake()
    {
      Transform parent;
      // Loop through the object prefabs and make a new list for each one.
      foreach (PoolGameObjectInfo poolObject in poolObjectList)
      {
        if (!pooledObjects.ContainsKey(poolObject.id))
        {
          pooledObjects.Add(poolObject.id, new List<GameObject>(poolObject.bufferCount));
          parent = poolObject.defaultParent != null ? poolObject.defaultParent : transform;

          // Init or complete the object pool
          for (int n = 0; n < poolObject.bufferCount; n++)
          {
            GameObject newObj = Instantiate(poolObject.prefab, parent) as GameObject;
            newObj.name = poolObject.id;
            Pool(newObj, poolObject.id, poolObject.defaultParent);
          }
        }
      }
    }

    void Start()
    {
      // Protection if HeavyAwake interface is not handled in the unity project
      // cf. FourScenesTemplate to know how to implement this interface
      if ((poolDatas == null || poolDatas.Count == 0) && LoopCount() > 0)
      {
        HeavyAwake();
      }
    }

    /// <summary>
    /// Returns the pooled object list for a specific kind. Object Pool manipulation purpose only
    /// </summary>
    /// <returns>The pooled object list.</returns>
    /// <param name="objectKind">Object kind.</param>
    public List<GameObject> GetPooledObjectList(string objectKind)
    {
      if (pooledObjects.ContainsKey(objectKind))
        return pooledObjects[objectKind];
      return null;
    }

    /// <summary>
    /// Gets a new object for the name type provided.  If no object of that type in the pool then <c>null</c> will be returned.
    /// </summary>
    /// <returns>
    /// The object for request prefab name.
    /// </returns>
    /// <param name='objectName'>
    /// Object prefab name
    /// </param>
    public GameObject GetFromPool(string objectName)
    {
      return GetFromPool(objectName, false);
    }

    /// <summary>
    /// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
    /// then null will be returned.
    /// </summary>
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='objectType'>
    /// Object type.
    /// </param>
    /// <param name='onlyPooled'>
    /// If true, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetFromPool(string objectName, bool onlyPooled)
    {
      return GetFromPool(objectName, onlyPooled, true);
    }

    GameObject InstantiateObject(PoolGameObjectInfo poolObject)
    {
      Transform parent = poolObject.defaultParent != null ? poolObject.defaultParent : transform;
      GameObject newObj = Instantiate(poolObject.prefab, parent) as GameObject;
      // Attach to object pool by default
      if (newObj.transform.parent == null)
      {
        newObj.transform.SetParent(transform);
      }
      newObj.transform.localScale = Vector3.one;
      newObj.name = poolObject.id;
      return newObj;
    }

    /// <summary>
    /// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
    /// then null will be returned.
    /// </summary>
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='objectType'>
    /// Object type.
    /// </param>
    /// <param name='onlyPooled'>
    /// If true, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetFromPool(string objectName, bool onlyPooled, bool activate)
    {
      PoolGameObjectInfo poolObject = poolObjectList.Find(element => objectName.StartsWith(element.id));
      bool objectPrefabExists = (poolObject != null);

#if UNITY_EDITOR
      // Don't get from pool while in editor and not playing
      if (!Application.isPlaying)
        return InstantiateObject(poolObject);
#endif

      if (objectPrefabExists)
      {
        // Get from the pool if there's one left
        if (pooledObjects.ContainsKey(objectName) && pooledObjects[objectName].Count > 0)
        {
          if (pooledObjects[objectName][0] != null)
          {
            GameObject pooledObject = pooledObjects[objectName][0];

            pooledObjects[objectName].RemoveAt(0);
            pooledObject.SetActive(activate);
            pooledObject.transform.SetParent(poolObject.defaultParent);
            return pooledObject;
          }

          return InstantiateObject(poolObject);
        }
        else if (!onlyPooled)
        {
          if (poolObject != null)
          {
            return InstantiateObject(poolObject);
          }

          return null;
        }
      }
      // If we have gotten here either there was no object of the specified type or none were left in the pool with onlyPooled set to true
      return null;
    }

    /// <summary>
    /// Create a new type of pooled object
    /// </summary>
    /// <param name="name"></param>
    /// <param name="prefab"></param>
    /// <param name="tag"></param>
    /// <param name="bufferCount"></param>
    /// <param name="maxCount"></param>
    /// <param name="defaultParent"></param>
    public void NewPoolKind(string name, GameObject prefab, string tag = "Default", int bufferCount = 1, int maxCount = -1, Transform defaultParent = null)
    {
      PoolGameObjectInfo poolObjectKind = poolObjectList.Find(element => name.Equals(element.id));
      if (poolObjectKind == null)
      {
        poolObjectKind = new PoolGameObjectInfo();
        poolObjectKind.bufferCount = bufferCount;
        poolObjectKind.defaultParent.SetParent(defaultParent);
        poolObjectKind.id = name;
        poolObjectKind.maxCount = maxCount;
        poolObjectKind.prefab = prefab;
        poolObjectList.Add(poolObjectKind);

        GameObject newObj = Instantiate(poolObjectKind.prefab) as GameObject;
        newObj.name = poolObjectKind.id;
        Pool(newObj, poolObjectKind.id, poolObjectKind.defaultParent);
      }
    }

    /// <summary>
    /// Pools the object specified.  Will not be pooled if there is no prefab of that type.
    /// </summary>
    /// <param name='obj'>
    /// Object to be pooled.
    /// </param>
    /// <returns>
    /// true if object could be pooled, false otherwise
    /// </returns>
    public bool Pool(GameObject obj, string objectKind = null, Transform poolParent = null)
    {

      if (obj == null)
        return false;

      if (objectKind == null)
        objectKind = obj.name;

      PoolGameObjectInfo poolObjectKind = poolObjectList.Find(element => objectKind.Equals(element.id));

      if (poolObjectKind != null)
      {
        bool objectPrefabListExists = pooledObjects.ContainsKey(poolObjectKind.id);
        // Create a new list for this kind of pooled objects (only known ones)
        if (!objectPrefabListExists)
          pooledObjects.Add(poolObjectKind.id, new List<GameObject>(1));
      }
      // reset its velocity if not kinematic
      if (obj.GetComponent<Rigidbody>() && !obj.GetComponent<Rigidbody>().isKinematic)
      {
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
      }
      // Never pool object in the editor, while not playing
      if (!Application.isPlaying)
        DestroyImmediate(obj);
      else
      {
        if (poolObjectKind == null || pooledObjects[poolObjectKind.id].Count > poolObjectKind.maxCount)
        {
          // Destroy an object that can't be pooled
          Destroy(obj);
        }
        else
        {
          // Deactivate the object, reset its position and scale
          obj.SetActive(false);
          // - set default parent or requested one
          obj.transform.SetParent(poolObjectKind.defaultParent);
          obj.transform.localPosition = Vector3.zero;
          obj.transform.localScale = Vector3.one;
          // Put the object in the object pool
          pooledObjects[poolObjectKind.id].Insert(0, obj);
        }
      }
      return true;
    }

    public void PoolFromTag(string tag)
    {
      PoolGameObjectList(GameObject.FindGameObjectsWithTag(tag));
    }
    public void PoolGameObjectList(GameObject[] goList)
    {
      for (int i = 0; i < goList.Length; i++)
      {
        Pool(goList[i], null, null);
      }
    }

    public void Pool(GameObject obj, float time)
    {
      poolDatas.Add(new PoolData(obj, Time.time + time));
    }

#if UNITY_EDITOR
    // Utility methods when in the editor

    [ContextMenu("Pool inactive objects")]
    public void PoolInactiveObjects()
    {
      for (int i = 0; i < poolObjectList.Count; i++)
      {
        PoolInactiveObjects(poolObjectList[i].id, poolObjectList[i].defaultParent);
      }
    }

    void PoolInactiveObjects(string namePrefix, Transform t)
    {
      if (t == null)
        return;
      // Recursively pool children first
      if (t.childCount > 0)
      {
        foreach (Transform child in t)
          PoolInactiveObjects(namePrefix, child);
      }
      // Then, if game object should be pooled (from its tag), Pool it
      if (t.gameObject.name.StartsWith(namePrefix))
      {
        Pool(t.gameObject);
      }
    }
#endif
  }
}