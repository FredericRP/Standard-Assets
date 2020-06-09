using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FredericRP.ObjectPooling
{
  [CustomEditor(typeof(ObjectPool))]
  public class ObjectPoolInspector : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      ObjectPool pool = target as ObjectPool;

      // New : set ID of ObjectPool
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
      pool.id = EditorGUILayout.TextField("Pool ID", pool.id);
      EditorGUILayout.EndHorizontal();
      //
      List<ObjectPool.PoolGameObjectInfo> poolObjectList = pool.PoolGameObjectInfoList;
      Color previousColor = GUI.color;
      //*
      EditorGUILayout.BeginHorizontal();
      GUILayout.Label("kind");
      //GUILayout.Label("R", GUILayout.Width(30));
      GUILayout.Label("init");
      GUILayout.Label("max");
      GUILayout.Label("----", GUILayout.Width(80));
      EditorGUILayout.EndHorizontal();

      EditorGUI.BeginChangeCheck();

      if (poolObjectList.Count > 0)
      {
        for (int i = 0; i < poolObjectList.Count; i++)
        {
          ObjectPool.PoolGameObjectInfo data = poolObjectList[i];
          EditorGUILayout.BeginHorizontal();

          // PoolObject : name, prefab, bufferCount, defaultParent
          data.id = EditorGUILayout.TextField(data.id);
          //data.tag = EditorGUILayout.TagField(data.tag);
          if (data.prefab == null)
            GUI.color = Color.red;
          data.prefab = (GameObject)EditorGUILayout.ObjectField(data.prefab, typeof(GameObject), false, GUILayout.Width(30));
          GUI.color = previousColor;

          data.bufferCount = EditorGUILayout.IntField(data.bufferCount, GUILayout.Width(38));
          float count = data.bufferCount;
          float max = data.maxCount;
          EditorGUILayout.MinMaxSlider(ref count, ref max, 0, 300);
          data.bufferCount = (int)count;
          data.maxCount = (int)max;
          data.maxCount = EditorGUILayout.IntField(data.maxCount, GUILayout.Width(38));

          GUILayout.Label("on");
          if (data.defaultParent == null)
            GUI.color = Color.red;
          data.defaultParent = (Transform)EditorGUILayout.ObjectField(data.defaultParent, typeof(Transform), true);
          GUI.color = previousColor;

          GUI.enabled = (i > 0);
          if (GUILayout.Button("\u25B2", EditorStyles.miniButtonLeft, GUILayout.Width(20)))
          {
            // Switch with previous property
            ObjectPool.PoolGameObjectInfo pgoInfo = poolObjectList[i];
            poolObjectList.RemoveAt(i);
            poolObjectList.Insert(i - 1, pgoInfo);

          }
          GUI.enabled = (i < poolObjectList.Count - 1);
          if (GUILayout.Button("\u25BC", EditorStyles.miniButtonMid, GUILayout.Width(20)))
          {
            // Switch with next property
            ObjectPool.PoolGameObjectInfo pgoInfo = poolObjectList[i];
            poolObjectList.RemoveAt(i);
            poolObjectList.Insert(i + 1, pgoInfo);
          }
          GUI.enabled = true;
          GUI.color = Color.green;
          if (GUILayout.Button("+", EditorStyles.miniButtonMid, GUILayout.Width(20)))
          {
            poolObjectList.Insert(i + 1, new ObjectPool.PoolGameObjectInfo());
          }
          GUI.color = Color.red;
          if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.Width(20)))
          {
            // Remove property
            poolObjectList.RemoveAt(i);
          }
          GUI.color = previousColor;
          EditorGUILayout.EndHorizontal();

        }
      }
      else
      {
        GUI.color = Color.green;
        if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
        {
          poolObjectList.Add(new ObjectPool.PoolGameObjectInfo());
        }
        GUI.color = previousColor;
      }

      if (EditorGUI.EndChangeCheck())
      {
        EditorUtility.SetDirty(pool);
      }
    }
  }
}