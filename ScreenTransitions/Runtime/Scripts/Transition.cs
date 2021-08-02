using FredericRP.EventManagement;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace FredericRP.ScreenTransitions
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
      transitionHiddenEvent.Raise();
    }

    [System.Obsolete("Use GetTransition(id).Show() instead")]
    public static void Show(string id = "default")
    {
      GetTransition(id).Show();
    }
    public void Show()
    {
#if UNITY_EDITOR && DEBUG
      Debug.Log(gameObject.name + " > Showing");
#endif
      animator?.SetBool("visible", true);
    }

    [System.Obsolete("Use GetTransition(id).Hide() instead")]
    public static void Hide(string id = "default")
    {
      GetTransition(id).Hide();
    }

    public void Hide()
    {
#if UNITY_EDITOR && DEBUG
      Debug.Log(gameObject.name + " > Hiding");
#endif
      animator?.SetBool("visible", false);
    }

    public void TriggerHidden()
    {
      transitionHiddenEvent.Raise();
    }
    public void TriggerDisplayed()
    {
      transitionShownEvent.Raise();
    }

#if UNITY_EDITOR
    private void Update()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
      if (Input.GetKeyDown(KeyCode.S))
      {
        Show();
      }
      if (Input.GetKeyDown(KeyCode.H))
      {
        Hide();
      }
#else
      if (Keyboard.current.sKey.wasPressedThisFrame)
      {
        Show();
      }
      if (Keyboard.current.hKey.wasPressedThisFrame)
      {
        Hide();
      }
#endif
    }
#endif
  }
}