using FredericRP.EventManagement;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventTrigger : MonoBehaviour
{
  [SerializeField]
  GameEvent gameEvent = null;
  [SerializeField]
  UnityEvent unityEvent = null;

  private void OnEnable()
  {
    EventHandler.AddEventListener(gameEvent, unityEvent.Invoke);
  }

  private void OnDisable()
  {
    EventHandler.RemoveEventListener(gameEvent, unityEvent.Invoke);
  }
}
