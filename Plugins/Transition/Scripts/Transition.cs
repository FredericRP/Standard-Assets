using FredericRP.EventManagement;
using UnityEngine;

namespace FredericRP.Transition
{
  public class Transition : Singleton<Transition>
  {
    [SerializeField]
    Animator animator = null;

    [SerializeField]
    GameEvent transitionHiddenEvent = null;
    [SerializeField]
    GameEvent transitionShownEvent = null;

    void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }

    private void Closed()
    {
      EventHandler.TriggerEvent(transitionHiddenEvent);
    }

    public static void Show()
    {
      Instance.animator.SetBool("visible", true);
    }
    public static void Hide()
    {
      Instance.animator?.SetBool("visible", false);
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