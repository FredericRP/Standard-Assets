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
    GameEventHandler.AddEventListener(gameEvent, unityEvent.Invoke);
  }

  private void OnDisable()
  {
    GameEventHandler.RemoveEventListener(gameEvent, unityEvent.Invoke);
  }
}
