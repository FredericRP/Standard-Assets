using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FredericRP.ObjectPooling;

namespace FredericRP.Popups
{

  public class PopupHandler : Singleton<PopupHandler>
  {
    [SerializeField]
    float delayBetweenPopupAnimation = 0.2f;
    [SerializeField]
    int defaultSortingLayer = 100;
    [SerializeField]
    string poolId = "pool";

    ObjectPool objectPool;

    private static List<PopupBase> popupList = new List<PopupBase>();

    /// <summary>
    /// The current popup descriptor, used to determine if a new popup should be shown
    /// </summary>
    PopupDescriptor currentPopupDescriptor;
    /// <summary>
    /// The current popup base component
    /// </summary>
    PopupBase currentPopup;

    void Start()
    {
      objectPool = ObjectPool.GetObjectPool(poolId);
    }

#if UNITY_EDITOR
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.T))
      {
        CloseAllPopups();
      }
    }
#endif
    /// <summary>
    /// Shows the specified popup.
    /// </summary>
    /// <param name="popup">Popup.</param>
    public static void ShowPopup(PopupDescriptor popup)
    {
      Instance.ShowPopup(popup, null);
    }

    /// <summary>
    /// Check if all parameters are equals or not
    /// </summary>
    /// <returns><c>true</c>, if parameters are equals, <c>false</c> otherwise.</returns>
    /// <param name="newParameters">New parameters.</param>
    /// <param name="oldParameters">Old parameters.</param>
    bool CheckParameterEquals(object newParameters, object oldParameters)
    {
      // If both parameters are null, it's equals
      if (newParameters == null && oldParameters == null)
        return true;
      // Otherwise, if one of them only is null, it's not equals
      if (newParameters == null || oldParameters == null)
        return false;

      // Otherwise, if it's not an array, check parameter equality
      if ((newParameters as object[]) == null)
      {
        return (newParameters.Equals(oldParameters));
      }
      // Otherwise, check each array object difference
      object[] newArray = (newParameters as object[]);
      object[] oldArray = (oldParameters as object[]);
      // If array have not the same length, it's different
      if (newArray.Length != oldArray.Length)
        return false;
      for (int i = 0; i < newArray.Length; i++)
      {
        bool result = CheckParameterEquals(newArray[i], oldArray[i]);
        // As soon as a different parameters has been found different, returns
        if (!result)
        {
          return false;
        }
      }
      // No difference has been found, return true;
      return true;
    }

    /// <summary>
    /// Shows the specified popup, using parameters
    /// </summary>
    /// <param name="popup">Popup.</param>
    /// <param name="parameters">Parameters.</param>
    public void ShowPopup(PopupDescriptor popup, object parameters)
    {
#if UNITY_EDITOR && DEBUG
      Debug.Log("Show popup " + popup + " / current " + currentPopupDescriptor);
#endif
      // If current popup is the same as the current shown one, with the same parameters, do nothing
      if (currentPopupDescriptor != null && currentPopupDescriptor.pooledObjectName == popup.pooledObjectName && (currentPopup != null && CheckParameterEquals(currentPopup.Parameters, parameters)))
      {
#if UNITY_EDITOR && DEBUG
        Debug.Log("Already showing with same parameters, abort !");
#endif
        return;
      }

      // Hide last popup to free the screen
      HideLastPopup();

      // Create the new popup from object pool
      PopupBase popupObject = (objectPool.GetFromPool(popup.pooledObjectName) as GameObject).GetComponent<PopupBase>();
      Debug.Log("Pooled " + popup.pooledObjectName + " => " + popupObject.name);
      // ensure new popup is above previous one
      if (currentPopup != null)
        popupObject.Canvas.sortingOrder = currentPopup.Canvas.sortingOrder + 1;
      else
        popupObject.Canvas.sortingOrder = defaultSortingLayer;
      // Initialize the new popup with specified parameters
      popupObject.Init(parameters);

      // Show the new popup
      currentPopupDescriptor = popup;
      currentPopup = popupObject;
      popupObject.Show();

      // Add it to the list
      popupList.Add(popupObject);
    }

    public static void ClosePopup()
    {
      Instance.StartCoroutine(Instance._closePopup());
    }

    private IEnumerator _closePopup()
    {
      currentPopupDescriptor = null;
      currentPopup = null;
      if (HideLastPopup())
      {
        // Should wait here
        yield return new WaitForSeconds(delayBetweenPopupAnimation);

        objectPool.Pool(popupList[popupList.Count - 1].gameObject);
        popupList.RemoveAt(popupList.Count - 1);

        if (popupList.Count > 0)
          popupList[popupList.Count - 1].Show();
      }
      else if (popupList.Count > 0)
      {
        // If we're here, that means there was no popup or the last popup was already hidden
        objectPool.Pool(popupList[popupList.Count - 1].gameObject);
        popupList.RemoveAt(popupList.Count - 1);

        if (popupList.Count > 0)
          popupList[popupList.Count - 1].Show();
      }
    }

    /// <summary>
    /// Closes all popups.
    /// </summary>
    public static void CloseAllPopups()
    {
      Instance.StartCoroutine(Instance._closeAllPopups());
    }

    private IEnumerator _closeAllPopups()
    {
      for (int i = popupList.Count - 1; i >= 0; i--)
      {
        if (popupList[i].IsVisible)
        {
          popupList[i].Hide();
          yield return new WaitForSeconds(delayBetweenPopupAnimation);
        }

        objectPool.Pool(popupList[i].gameObject);
      }

      popupList.Clear();
    }

    /// <summary>
    /// Hides the last popup.
    /// </summary>
    /// <returns><c>true</c>, if last popup is hiding, <c>false</c> if there was no popup to hide.</returns>
    private static bool HideLastPopup()
    {
      PopupBase lastPopup = null;

      if (popupList.Count > 0)
        lastPopup = popupList[popupList.Count - 1];

      if (lastPopup != null && lastPopup.IsVisible)
      {
        // TODO : add callback on show/hide
        lastPopup.Hide();
        return true;
      }

      return false;
    }
  }
}