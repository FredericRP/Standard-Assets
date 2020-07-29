using FredericRP.EventManagement;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.Transition
{
  public class Transition : MonoBehaviour
  {
    [SerializeField]
    Animator animator = null;

    [SerializeField]
    GameEvent transitionHiddenEvent = null;
    [SerializeField]
    GameEvent transitionShownEvent = null;

    protected static List<Transition> transitionList;
    [SerializeField]
    string id = "default";

    void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
      if (transitionList == null)
        transitionList = new List<Transition>();
      // Auto assign
      transitionList.Add(this);
    }

    private void OnDisable()
    {
      transitionList.Remove(this);
    }

    public static Transition GetTransition(string id = "default")
    {
      if (transitionList != null)
      {
        int index = transitionList.FindIndex(transition => transition.id == id);
        if (index >= 0)
          return transitionList[index];
      }
      return null;
    }

    private void Closed()
    {
      EventHandler.TriggerEvent(transitionHiddenEvent);
    }

    [System.Obsolete("Use GetTransition(id).Show() instead")]
    public static void Show(string id = "default")
    {
      GetTransition(id).Show();
    }
    public void Show()
    {
      Debug.Log(gameObject.name + " > Showing");
      animator?.SetBool("visible", true);
    }

    [System.Obsolete("Use GetTransition(id).Hide() instead")]
    public static void Hide(string id = "default")
    {
      GetTransition(id).Hide();
    }

    public void Hide()
    {
      Debug.Log(gameObject.name + " > Hiding");
      animator?.SetBool("visible", false);
    }

    public void TriggerHidden()
    {
      EventHandler.TriggerEvent(transitionHiddenEvent);
    }
    public void TriggerDisplayed()
    {
      EventHandler.TriggerEvent(transitionShownEvent);
    }

    private void Update()
    {
#if UNITY_EDITOR
      if (Input.GetKeyDown(KeyCode.S))
      {
        Show();
      }
      if (Input.GetKeyDown(KeyCode.H))
      {
        Hide();
      }
#endif
    }
  }
}